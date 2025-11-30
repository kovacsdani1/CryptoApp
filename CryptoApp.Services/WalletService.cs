using AutoMapper;
using CryptoApp.DataContext.Dtos;
using CryptoApp.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

public interface IWalletService
{
    Task<WalletDto> GetWalletAsync(int userId);
    Task<WalletDto> CreateWalletAsync(int userId, decimal initialBalance);
    Task<WalletDto> UpdateBalanceAsync(int userId, decimal newBalance);
    Task DeleteWalletAsync(int userId);
}

public class WalletService : IWalletService
{
    private readonly CryptoDbContext _context;
    private readonly IMapper _mapper;

    public WalletService(CryptoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WalletDto> GetWalletAsync(int userId)
    {
        var wallet = await _context.Wallets
            .Include(w => w.Holdings)
            .ThenInclude(h => h.Cryptocurrency)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        var walletDto = _mapper.Map<WalletDto>(wallet);

        foreach (var holding in walletDto.Holdings)
        {
            holding.TotalValue = holding.Amount * holding.CurrentPrice;
        }

        return walletDto;
    }

    public async Task<WalletDto> CreateWalletAsync(int userId, decimal initialBalance)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (await _context.Wallets.AnyAsync(w => w.UserId == userId))
        {
            throw new InvalidOperationException("Wallet already exists for this user.");
        }

        var wallet = new Wallet
        {
            UserId = userId,
            Balance = initialBalance,
            Holdings = new List<CryptoHolding>()
        };

        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();

        return _mapper.Map<WalletDto>(wallet);
    }

    public async Task<WalletDto> UpdateBalanceAsync(int userId, decimal newBalance)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        wallet.Balance = newBalance;
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();

        return _mapper.Map<WalletDto>(wallet);
    }

    public async Task DeleteWalletAsync(int userId)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync();
    }
}