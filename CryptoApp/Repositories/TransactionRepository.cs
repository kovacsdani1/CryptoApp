using CryptoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
        Task<Transaction> GetTransactionByIdAsync(int transactionId);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CryptoDbContext _context;
        public TransactionRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.Transactions.Include(t => t.Crypto).Include(t=>t.User).FirstOrDefaultAsync(t=>t.Id==transactionId);
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            return await _context.Transactions.Include(t => t.Crypto).Where(t => t.UserId == userId).ToListAsync();
        }

    }
}
