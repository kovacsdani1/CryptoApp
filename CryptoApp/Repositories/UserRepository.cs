using CryptoApp.Entities;
using CryptoApp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<User> GetUserAsync(int id);
        Task UpdateUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly CryptoDbContext _context;
        public UserRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User user)
        {
            if(await _context.Users.AnyAsync(u=>u.Email == user.Email))
                throw new Exception("User with this email already exists.");

            await _context.Users.AddAsync(user);
            await _context.Wallets.AddAsync(new Wallet { User = user, Balance = 10000 });
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            User oldUser = await _context.Users.FindAsync(id);

            oldUser.Email = user.Email;
            oldUser.Username = user.Username;
            oldUser.Password = user.Password;
        }

        public async Task DeleteUserAsync(int id)
        {
            User user = await _context.Users.FindAsync(id);

            if(user == null)
                throw new NotFoundException("User", id);

            _context.Users.Remove(user);
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
