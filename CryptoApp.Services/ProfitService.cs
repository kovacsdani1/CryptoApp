using AutoMapper;
using CryptoApp.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;

public interface IProfitService
{
    Task<ProfitSummaryDto> CalculateProfitAsync(int userId);
    Task<List<DetailedProfitDto>> CalculateDetailedProfitAsync(int userId);
}

public class ProfitService : IProfitService
{
    private readonly CryptoDbContext _context;
    private readonly IMapper _mapper;

    public ProfitService(CryptoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProfitSummaryDto> CalculateProfitAsync(int userId)
    {
        var wallet = await _context.Wallets
            .Include(w => w.Holdings)
            .ThenInclude(h => h.Cryptocurrency)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        decimal totalInvestment = 0;
        decimal currentValue = 0;

        foreach (var holding in wallet.Holdings)
        {
            decimal investmentForThisHolding = holding.Amount * holding.AveragePurchasePrice;
            decimal currentValueForThisHolding = holding.Amount * holding.Cryptocurrency.CurrentPrice;

            totalInvestment += investmentForThisHolding;
            currentValue += currentValueForThisHolding;
        }

        decimal totalProfit = currentValue - totalInvestment;
        decimal profitPercentage = totalInvestment == 0 ? 0 : (totalProfit / totalInvestment) * 100;

        return new ProfitSummaryDto
        {
            TotalInvestment = totalInvestment,
            CurrentValue = currentValue,
            TotalProfit = totalProfit,
            ProfitPercentage = profitPercentage
        };
    }

    public async Task<List<DetailedProfitDto>> CalculateDetailedProfitAsync(int userId)
    {
        var wallet = await _context.Wallets
            .Include(w => w.Holdings)
            .ThenInclude(h => h.Cryptocurrency)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            throw new KeyNotFoundException("Wallet not found.");
        }

        var detailedProfits = new List<DetailedProfitDto>();

        foreach (var holding in wallet.Holdings)
        {
            decimal investmentForThisHolding = holding.Amount * holding.AveragePurchasePrice;
            decimal currentValueForThisHolding = holding.Amount * holding.Cryptocurrency.CurrentPrice;
            decimal profitForThisHolding = currentValueForThisHolding - investmentForThisHolding;
            decimal profitPercentage = investmentForThisHolding == 0 ? 0 : (profitForThisHolding / investmentForThisHolding) * 100;

            detailedProfits.Add(new DetailedProfitDto
            {
                CryptocurrencyId = holding.CryptocurrencyId,
                Name = holding.Cryptocurrency.Name,
                Symbol = holding.Cryptocurrency.Symbol,
                Amount = holding.Amount,
                AveragePurchasePrice = holding.AveragePurchasePrice,
                CurrentPrice = holding.Cryptocurrency.CurrentPrice,
                TotalInvestment = investmentForThisHolding,
                CurrentValue = currentValueForThisHolding,
                Profit = profitForThisHolding,
                ProfitPercentage = profitPercentage
            });
        }

        return detailedProfits;
    }
}