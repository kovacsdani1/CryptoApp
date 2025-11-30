using AutoMapper;
using BCrypt.Net;
using CryptoApp.DataContext.Dtos;
using CryptoApp.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserRegisterDto userDto);
    Task<string> LoginAsync(UserLoginDto userDto);
    Task<UserDto> GetUserByIdAsync(int userId);
    Task<UserDto> UpdateUserAsync(int userId, UserRegisterDto userDto);
    Task DeleteUserAsync(int userId);
}

public class UserService : IUserService
{
    private readonly CryptoDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IWalletService _walletService;

    public UserService(
        CryptoDbContext context,
        IMapper mapper,
        IConfiguration configuration,
        IWalletService walletService)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
        _walletService = walletService;
    }

    public async Task<UserDto> RegisterAsync(UserRegisterDto userDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
        {
            throw new InvalidOperationException("Email already in use.");
        }

        var user = _mapper.Map<User>(userDto);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _walletService.CreateWalletAsync(user.Id, 10000); //10000 kezdő balance

        return _mapper.Map<UserDto>(user);
    }

    public async Task<string> LoginAsync(UserLoginDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        return GenerateJwtToken(user);
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUserAsync(int userId, UserRegisterDto userDto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.Username = userDto.Username;
        user.Email = userDto.Email;
        if (!string.IsNullOrEmpty(userDto.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}