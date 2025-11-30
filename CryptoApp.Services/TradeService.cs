using AutoMapper;
using CryptoApp.DataContext.Dtos;
using CryptoApp.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

public interface ITradeService
{
    Task<TransactionDto> BuyCryptoAsync(TradeRequestDto request);
    Task<TransactionDto> SellCryptoAsync(TradeRequestDto request);
    Task<List<CryptoHoldingDto>> GetPortfolioAsync(int userId);
}

public class TradeService : ITradeService
{
    private readonly CryptoDbContext _context;
    private readonly IMapper _mapper;

    public TradeService(CryptoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TransactionDto> BuyCryptoAsync(TradeRequestDto request)
    {
        var wallet = await _context.Wallets
            .Include(w => w.Holdings)
            .FirstOrDefaultAsync(w => w.UserId == request.UserId);

        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        var crypto = await _context.Cryptocurrencies.FindAsync(request.CryptocurrencyId);
        if (crypto == null)
        {
            throw new KeyNotFoundException("Cryptocurrency not found.");
        }

        decimal totalCost = request.Amount * crypto.CurrentPrice;

        if (wallet.Balance < totalCost)
        {
            throw new InvalidOperationException("Insufficient balance.");
        }

        wallet.Balance -= totalCost;

        var holding = wallet.Holdings.FirstOrDefault(h => h.CryptocurrencyId == request.CryptocurrencyId);
        if (holding == null)
        {
            holding = new CryptoHolding
            {
                WalletId = wallet.Id,
                CryptocurrencyId = request.CryptocurrencyId,
                Amount = request.Amount,
                AveragePurchasePrice = crypto.CurrentPrice
            };
            await _context.CryptoHoldings.AddAsync(holding);
        }
        else
        {
            //update average purchase price
            decimal totalValue = (holding.Amount * holding.AveragePurchasePrice) + totalCost;
            holding.Amount += request.Amount;
            holding.AveragePurchasePrice = totalValue / holding.Amount;
            _context.CryptoHoldings.Update(holding);
        }

        var transaction = new Transaction
        {
            UserId = request.UserId,
            CryptocurrencyId = request.CryptocurrencyId,
            Type = TransactionType.Buy,
            Amount = request.Amount,
            Price = crypto.CurrentPrice,
            TotalValue = totalCost,
            Timestamp = DateTime.UtcNow
        };

        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();

        return _mapper.Map<TransactionDto>(transaction);
    }

    public async Task<TransactionDto> SellCryptoAsync(TradeRequestDto request)
    {
        var wallet = await _context.Wallets
            .Include(w => w.Holdings)
            .FirstOrDefaultAsync(w => w.UserId == request.UserId);

        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        var crypto = await _context.Cryptocurrencies.FindAsync(request.CryptocurrencyId);
        if (crypto == null)
        {
            throw new KeyNotFoundException("Cryptocurrency not found.");
        }

        var holding = wallet.Holdings.FirstOrDefault(h => h.CryptocurrencyId == request.CryptocurrencyId);
        if (holding == null || holding.Amount < request.Amount)
        {
            throw new InvalidOperationException("Insufficient cryptocurrency amount.");
        }

        decimal totalValue = request.Amount * crypto.CurrentPrice;

        wallet.Balance += totalValue;

        holding.Amount -= request.Amount;
        if (holding.Amount == 0)
        {
            _context.CryptoHoldings.Remove(holding);
        }
        else
        {
            _context.CryptoHoldings.Update(holding);
        }

        var transaction = new Transaction
        {
            UserId = request.UserId,
            CryptocurrencyId = request.CryptocurrencyId,
            Type = TransactionType.Sell,
            Amount = request.Amount,
            Price = crypto.CurrentPrice,
            TotalValue = totalValue,
            Timestamp = DateTime.UtcNow
        };

        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();

        return _mapper.Map<TransactionDto>(transaction);
    }

    public async Task<List<CryptoHoldingDto>> GetPortfolioAsync(int userId)
    {
        var wallet = await _context.Wallets
            .Include(w => w.Holdings)
            .ThenInclude(h => h.Cryptocurrency)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        var holdingDtos = _mapper.Map<List<CryptoHoldingDto>>(wallet.Holdings);

        foreach (var holding in holdingDtos)
        {
            holding.TotalValue = holding.Amount * holding.CurrentPrice;
        }

        return holdingDtos;
    }
}