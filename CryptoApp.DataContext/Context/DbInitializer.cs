using BCrypt.Net;
using CryptoApp.DataContext.Entities;
using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static async Task Initialize(CryptoDbContext context)
    {
        // Adatbázis migrálása, ha még nem történt meg
        await context.Database.MigrateAsync();

        // Check if DB has been seeded already
        if (context.Cryptocurrencies.Any())
        {
            return;   // DB has been seeded
        }

        // Seed cryptocurrencies
        var cryptocurrencies = new List<Cryptocurrency>
        {
            new Cryptocurrency { Name = "Bitcoin", Symbol = "BTC", CurrentPrice = 55000 },
            new Cryptocurrency { Name = "Ethereum", Symbol = "ETH", CurrentPrice = 3500 },
            new Cryptocurrency { Name = "Binance Coin", Symbol = "BNB", CurrentPrice = 500 },
            new Cryptocurrency { Name = "Cardano", Symbol = "ADA", CurrentPrice = 2.0m },
            new Cryptocurrency { Name = "Solana", Symbol = "SOL", CurrentPrice = 150 },
            new Cryptocurrency { Name = "XRP", Symbol = "XRP", CurrentPrice = 1.1m },
            new Cryptocurrency { Name = "Polkadot", Symbol = "DOT", CurrentPrice = 30 },
            new Cryptocurrency { Name = "Dogecoin", Symbol = "DOGE", CurrentPrice = 0.25m },
            new Cryptocurrency { Name = "Avalanche", Symbol = "AVAX", CurrentPrice = 75 },
            new Cryptocurrency { Name = "Terra", Symbol = "LUNA", CurrentPrice = 40 },
            new Cryptocurrency { Name = "Chainlink", Symbol = "LINK", CurrentPrice = 25 },
            new Cryptocurrency { Name = "Litecoin", Symbol = "LTC", CurrentPrice = 180 },
            new Cryptocurrency { Name = "Polygon", Symbol = "MATIC", CurrentPrice = 1.5m },
            new Cryptocurrency { Name = "Algorand", Symbol = "ALGO", CurrentPrice = 1.7m },
            new Cryptocurrency { Name = "Cosmos", Symbol = "ATOM", CurrentPrice = 28 }
        };

        context.Cryptocurrencies.AddRange(cryptocurrencies);
        await context.SaveChangesAsync();

        // Create price history entries for each cryptocurrency
        var now = DateTime.UtcNow;
        var priceHistories = new List<PriceHistory>();

        foreach (var crypto in cryptocurrencies)
        {
            // Add 10 price history entries for each crypto with slightly different prices for historical data
            for (int i = 0; i < 10; i++)
            {
                // Generate a random price variation between -5% and +5%
                Random random = new Random();
                decimal variation = (decimal)(random.NextDouble() * 0.1 - 0.05);
                decimal historicalPrice = Math.Round(crypto.CurrentPrice * (1 + variation), 2);

                priceHistories.Add(new PriceHistory
                {
                    CryptocurrencyId = crypto.Id,
                    Price = historicalPrice,
                    Timestamp = now.AddHours(-i) // Historical data going back in time
                });
            }
        }

        context.PriceHistories.AddRange(priceHistories);
        await context.SaveChangesAsync();

        // Seed test users
        var users = new List<User>
        {
            new User
            {
                Username = "testuser1",
                Email = "test1@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123")
            },
            new User
            {
                Username = "testuser2",
                Email = "test2@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123")
            },
            new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123")
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // Create wallets for users
        var wallets = new List<Wallet>();
        foreach (var user in users)
        {
            wallets.Add(new Wallet
            {
                UserId = user.Id,
                Balance = 10000 // Start with 10,000 currency units
            });
        }

        context.Wallets.AddRange(wallets);
        await context.SaveChangesAsync();

        // Seed some initial transactions and holdings for the first user
        var firstUser = users[0];
        var firstWallet = wallets[0];
        var transactions = new List<Transaction>();
        var holdings = new List<CryptoHolding>();

        // Buy some Bitcoin
        var bitcoin = cryptocurrencies[0];
        decimal btcAmount = 0.1m;
        decimal btcPrice = bitcoin.CurrentPrice;
        decimal btcCost = btcAmount * btcPrice;

        transactions.Add(new Transaction
        {
            UserId = firstUser.Id,
            CryptocurrencyId = bitcoin.Id,
            Type = TransactionType.Buy,
            Amount = btcAmount,
            Price = btcPrice,
            TotalValue = btcCost,
            Timestamp = now.AddDays(-5)
        });

        holdings.Add(new CryptoHolding
        {
            WalletId = firstWallet.Id,
            CryptocurrencyId = bitcoin.Id,
            Amount = btcAmount,
            AveragePurchasePrice = btcPrice
        });

        // Buy some Ethereum
        var ethereum = cryptocurrencies[1];
        decimal ethAmount = 1.5m;
        decimal ethPrice = ethereum.CurrentPrice;
        decimal ethCost = ethAmount * ethPrice;

        transactions.Add(new Transaction
        {
            UserId = firstUser.Id,
            CryptocurrencyId = ethereum.Id,
            Type = TransactionType.Buy,
            Amount = ethAmount,
            Price = ethPrice,
            TotalValue = ethCost,
            Timestamp = now.AddDays(-3)
        });

        holdings.Add(new CryptoHolding
        {
            WalletId = firstWallet.Id,
            CryptocurrencyId = ethereum.Id,
            Amount = ethAmount,
            AveragePurchasePrice = ethPrice
        });

        // Update wallet balance
        firstWallet.Balance -= (btcCost + ethCost);

        context.Transactions.AddRange(transactions);
        context.CryptoHoldings.AddRange(holdings);
        context.Wallets.Update(firstWallet);
        await context.SaveChangesAsync();
    }
}