using CryptoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Repositories
{
    public interface ICryptoRepository
    {
        Task<List<Crypto>> GetAllCryptosAsync();
        Task<Crypto> GetCryptoByIdAsync(int cryptoId);
        Task AddCryptoAsync(Crypto crypto);
        Task<bool> IsCryptoSymbolExistsAsync(string symbol);
        Task DeleteCryptoAsync(int cryptoId);
        Task<Crypto> GetCryptoBySymbolAsync(string symbol);
        Task UpdatePriceAsync(Crypto crypto, decimal newPrice);
        Task<List<PriceHistory>> GetPriceHistoryByIdAsync(int cryptoId);
    }
    public class CryptoRepository : ICryptoRepository
    {
        private readonly CryptoDbContext _context;
        public CryptoRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task AddCryptoAsync(Crypto crypto)
        {
            await _context.Cryptos.AddAsync(crypto);
        }

        public async Task<bool> IsCryptoSymbolExistsAsync(string symbol)
        {
            return await _context.Cryptos.AnyAsync(c=>c.Symbol == symbol);
        }

        public async Task<List<Crypto>> GetAllCryptosAsync()
        {
            return await _context.Cryptos.ToListAsync();
        }

        public async Task<Crypto> GetCryptoByIdAsync(int cryptoId)
        {
            return await _context.Cryptos.FirstOrDefaultAsync(c=> c.Id == cryptoId);
        }

        public async Task DeleteCryptoAsync(int cryptoId)
        {
            var crypto = await _context.Cryptos.FirstOrDefaultAsync(c=>c.Id==cryptoId);
            _context.Cryptos.Remove(crypto);
        }

        public async Task<Crypto> GetCryptoBySymbolAsync(string symbol)
        {
            return await _context.Cryptos.FirstOrDefaultAsync(c=> c.Symbol == symbol);
        }

        public async Task UpdatePriceAsync(Crypto crypto, decimal newPrice)
        {
            crypto.Price = newPrice;

            await _context.PriceHistories.AddAsync(new PriceHistory
            {
                CryptoId = crypto.Id,
                NewPrice = newPrice,
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task<List<PriceHistory>> GetPriceHistoryByIdAsync(int cryptoId)
        {
            return await _context.PriceHistories
                .Where(ph => ph.CryptoId == cryptoId).Include(ph=>ph.Crypto)
                .ToListAsync();
        }
    }
}
