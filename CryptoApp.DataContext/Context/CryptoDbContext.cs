using CryptoApp.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class CryptoDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
    public DbSet<CryptoHolding> CryptoHoldings { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }

    public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Transactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Wallet>()
            .HasMany(w => w.Holdings)
            .WithOne(h => h.Wallet)
            .HasForeignKey(h => h.WalletId);

        modelBuilder.Entity<Cryptocurrency>()
            .HasMany(c => c.PriceHistory)
            .WithOne(ph => ph.Cryptocurrency)
            .HasForeignKey(ph => ph.CryptocurrencyId);

        base.OnModelCreating(modelBuilder);
    }
}