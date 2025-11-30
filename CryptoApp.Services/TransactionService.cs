using AutoMapper;
using CryptoApp.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetUserTransactionsAsync(int userId);
    Task<TransactionDto> GetTransactionDetailsAsync(int transactionId);
}

public class TransactionService : ITransactionService
{
    private readonly CryptoDbContext _context;
    private readonly IMapper _mapper;

    public TransactionService(CryptoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> GetUserTransactionsAsync(int userId)
    {
        var transactions = await _context.Transactions
            .Include(t => t.Cryptocurrency)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync();

        return _mapper.Map<List<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> GetTransactionDetailsAsync(int transactionId)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Cryptocurrency)
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null)
        {
            throw new KeyNotFoundException("Transaction not found.");
        }

        return _mapper.Map<TransactionDto>(transaction);
    }
}