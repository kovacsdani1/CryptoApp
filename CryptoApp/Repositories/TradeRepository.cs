using CryptoApp.DTOs;
using CryptoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Repositories
{
    public interface ITradeRepository
    {
        Task BuyAsync(int userId, Crypto crypto, Wallet wallet, decimal amountToBuy, decimal budget);
        Task SellAsync(int userId, Crypto crypto, Wallet wallet, decimal amount, Portfolio portfolio);
    }
    public class TradeRepository : ITradeRepository
    {
        private readonly CryptoDbContext _context;
        public TradeRepository(CryptoDbContext context)
        {
            _context = context;
        }
        public async Task BuyAsync(int userId, Crypto crypto, Wallet wallet, decimal amountToBuy, decimal budget)
        {
            var portfolio = await _context.Portfolios.Where(p => p.WalletId == wallet.Id && p.CryptoId == crypto.Id).FirstOrDefaultAsync();
            if (portfolio != null)
                portfolio.Amount += amountToBuy;
            else
                await _context.Portfolios.AddAsync(new Portfolio
                {
                    Amount = amountToBuy,
                    WalletId = wallet.Id,
                    CryptoId = crypto.Id
                });

            await _context.Transactions.AddAsync(new Transaction
            {
                Amount = amountToBuy,
                CurrentPrice = crypto.Price,
                Date = DateTime.UtcNow,
                Type = TransactionType.Buy,
                UserId = userId,
                CryptoId = crypto.Id
            });

            wallet.Balance -= budget;
            crypto.Supply -= amountToBuy;
        }

        public async Task SellAsync(int userId, Crypto crypto, Wallet wallet, decimal amount, Portfolio portfolio)
        {
            portfolio.Amount -= amount;
            if (portfolio.Amount == 0)
                _context.Portfolios.Remove(portfolio);

            await _context.Transactions.AddAsync(new Transaction
            {
                Amount = amount,
                CurrentPrice = crypto.Price,
                Date = DateTime.UtcNow,
                Type = TransactionType.Sell,
                UserId = userId,
                CryptoId = crypto.Id
            });

            var budget = amount * crypto.Price;

            wallet.Balance += budget;
            crypto.Supply += amount;
        }
    }
}
