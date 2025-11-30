using CryptoApp.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class PriceUpdateService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PriceUpdateService> _logger;
    private readonly Random _random = new Random();
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

    public PriceUpdateService(
        IServiceProvider serviceProvider,
        ILogger<PriceUpdateService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Price Update Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Price Update Service is running at: {time}", DateTimeOffset.Now);

            try
            {
                await UpdatePrices();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating prices.");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Price Update Service is stopping.");
    }

    private async Task UpdatePrices()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();
            var cryptocurrencies = await context.Cryptocurrencies.ToListAsync();

            foreach (var crypto in cryptocurrencies)
            {
                //Árváltozás kiszámítása
                decimal changePercentage = (decimal)(_random.NextDouble() * 10 - 5);
                decimal priceChange = crypto.CurrentPrice * (changePercentage / 100);

                //Ár nem mehet egy minimum alá
                crypto.CurrentPrice = Math.Max(0.01m, crypto.CurrentPrice + priceChange);

                var priceHistory = new PriceHistory
                {
                    CryptocurrencyId = crypto.Id,
                    Price = crypto.CurrentPrice,
                    Timestamp = DateTime.UtcNow
                };

                await context.PriceHistories.AddAsync(priceHistory);
            }

            await context.SaveChangesAsync();
        }
    }
}