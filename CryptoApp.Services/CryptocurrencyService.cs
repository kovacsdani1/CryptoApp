using AutoMapper;
using CryptoApp.DataContext.Dtos;
using CryptoApp.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

public interface ICryptocurrencyService
{
    Task<List<CryptocurrencyDto>> GetAllCryptocurrenciesAsync();
    Task<CryptocurrencyDto> GetCryptocurrencyByIdAsync(int cryptoId);
    Task<CryptocurrencyDto> CreateCryptocurrencyAsync(CryptocurrencyDto cryptoDto);
    Task DeleteCryptocurrencyAsync(int cryptoId);
    Task<CryptocurrencyDto> UpdatePriceAsync(int cryptoId, decimal newPrice);
    Task<List<PriceHistory>> GetPriceHistoryAsync(int cryptoId);
}

public class CryptocurrencyService : ICryptocurrencyService
{
    private readonly CryptoDbContext _context;
    private readonly IMapper _mapper;

    public CryptocurrencyService(CryptoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CryptocurrencyDto>> GetAllCryptocurrenciesAsync()
    {
        var cryptos = await _context.Cryptocurrencies.ToListAsync();
        return _mapper.Map<List<CryptocurrencyDto>>(cryptos);
    }

    public async Task<CryptocurrencyDto> GetCryptocurrencyByIdAsync(int cryptoId)
    {
        var crypto = await _context.Cryptocurrencies.FindAsync(cryptoId);
        if (crypto == null)
        {
            throw new KeyNotFoundException("Cryptocurrency not found.");
        }

        return _mapper.Map<CryptocurrencyDto>(crypto);
    }

    public async Task<CryptocurrencyDto> CreateCryptocurrencyAsync(CryptocurrencyDto cryptoDto)
    {
        var crypto = _mapper.Map<Cryptocurrency>(cryptoDto);

        await _context.Cryptocurrencies.AddAsync(crypto);
        await _context.SaveChangesAsync();

        var priceHistory = new PriceHistory
        {
            CryptocurrencyId = crypto.Id,
            Price = crypto.CurrentPrice,
            Timestamp = DateTime.UtcNow
        };

        await _context.PriceHistories.AddAsync(priceHistory);
        await _context.SaveChangesAsync();

        return _mapper.Map<CryptocurrencyDto>(crypto);
    }

    public async Task DeleteCryptocurrencyAsync(int cryptoId)
    {
        var crypto = await _context.Cryptocurrencies.FindAsync(cryptoId);
        if (crypto == null)
        {
            throw new KeyNotFoundException("Cryptocurrency not found.");
        }

        _context.Cryptocurrencies.Remove(crypto);
        await _context.SaveChangesAsync();
    }

    public async Task<CryptocurrencyDto> UpdatePriceAsync(int cryptoId, decimal newPrice)
    {
        var crypto = await _context.Cryptocurrencies.FindAsync(cryptoId);
        if (crypto == null)
        {
            throw new KeyNotFoundException("Cryptocurrency not found.");
        }

        crypto.CurrentPrice = newPrice;
        _context.Cryptocurrencies.Update(crypto);

        var priceHistory = new PriceHistory
        {
            CryptocurrencyId = cryptoId,
            Price = newPrice,
            Timestamp = DateTime.UtcNow
        };

        await _context.PriceHistories.AddAsync(priceHistory);
        await _context.SaveChangesAsync();

        return _mapper.Map<CryptocurrencyDto>(crypto);
    }

    public async Task<List<PriceHistory>> GetPriceHistoryAsync(int cryptoId)
    {
        var history = await _context.PriceHistories
            .Where(ph => ph.CryptocurrencyId == cryptoId)
            .OrderByDescending(ph => ph.Timestamp)
            .ToListAsync();

        return history;
    }
}