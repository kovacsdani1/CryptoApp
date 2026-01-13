using CryptoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Repositories
{
    public interface IProfitRepository
    {
        Task<decimal> CalulateProfitAsync(int userId);
        Task<List<ProfitDto>> CalulcateProfitDetailsAsync(int userId);
    }
    public class ProfitRepository : IProfitRepository
    {
        private readonly CryptoDbContext _context;
        public ProfitRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalulateProfitAsync(int userId)
        {
            List<ProfitDto> profitDetails = await CalulcateProfitDetailsAsync(userId);
            decimal sumProfit = profitDetails.Sum(p => p.profit);
            return sumProfit;
        }

        public async Task<List<ProfitDto>> CalulcateProfitDetailsAsync(int userId)
        {
            List<ProfitDto> profitDetails = new List<ProfitDto>();

            var cryptos = await _context.Cryptos.ToListAsync(); // all cryptos
            var walletid = await _context.Wallets.Where(w => w.UserId == userId).Select(w => w.Id).FirstOrDefaultAsync(); //user's wallet id
            var portfolio = await _context.Portfolios.Where(p => p.WalletId == walletid).ToListAsync(); //user's portfolio
            var transactions = await _context.Transactions.Where(t => t.UserId == userId && t.Type == TransactionType.Buy).ToListAsync(); //user's buy transactions

            foreach (var item in portfolio)
            {
                int cryptoId = item.CryptoId;
                decimal currentPrice = cryptos.Where(c => c.Id == cryptoId).Select(c => c.Price).FirstOrDefault(); //current price of the crypto (item)
                //how much user spend on buying this crypto
                decimal spent = transactions.Where(t => t.CryptoId == cryptoId && t.UserId == userId).Sum(t => t.CurrentPrice * t.Amount);
                //the avg buy price of the crypto (all money spent / total crypto amount bought)
                decimal avgBuyPrice = spent / transactions.Where(t => t.CryptoId == cryptoId).Sum(t => t.Amount);
                //profit for this crypto
                decimal profit = (currentPrice - avgBuyPrice) * item.Amount;
                profitDetails.Add(new ProfitDto
                {
                    cryptoName = cryptos.Where(c => c.Id == cryptoId).Select(c => c.Symbol).FirstOrDefault(),
                    profit = profit
                });
            }

            return profitDetails;
        }
    }
}
