using CryptoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet> GetWalletAsync(int userId);
        Task UpdateBalanceAsync(int userId, decimal amount);
        Task DeleteWalletAsync(int userId);
    }
    public class WalletRepository : IWalletRepository
    {
        private readonly CryptoDbContext _context;
        public WalletRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetWalletAsync(int userId)
        {
            return await _context.Wallets.Include(w=>w.Portfolios).FirstOrDefaultAsync(w=>w.UserId==userId);
        }

        public async Task UpdateBalanceAsync(int userId, decimal amount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w=>w.UserId==userId);
            wallet.Balance = amount;
        }

        public async Task DeleteWalletAsync(int userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            _context.Wallets.Remove(wallet);
        }
    }
}
