
using CryptoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public PriceUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();

                var cryptos = await context.Cryptos.ToListAsync();
                var random = new Random();
                foreach (var crypto in cryptos)
                {
                    // price change between -5% to +5%
                    var percentageChange = (decimal)(random.NextDouble() * 0.1 - 0.05);
                    var newPrice = crypto.Price + (crypto.Price * percentageChange);
                    newPrice = Math.Round(newPrice, 2);
                    if(newPrice < 0) newPrice = 0;
                    crypto.Price = newPrice;

                    var priceHistory = new PriceHistory
                    {
                        CryptoId = crypto.Id,
                        NewPrice = newPrice,
                        Timestamp = DateTime.UtcNow
                    };
                    await context.PriceHistories.AddAsync(priceHistory);
                }
                await context.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
