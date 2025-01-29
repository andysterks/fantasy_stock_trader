using FantasyStockTrader.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FantasyStockTrader.Core.DatabaseContext;

public class FantasyStockTraderContext : DbContext
{

    public FantasyStockTraderContext(DbContextOptions<FantasyStockTraderContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Session>()
            .Property(s => s.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Session>()
            .HasIndex(s => s.RefreshToken)
            .HasDatabaseName("UXC_Session_RefreshToken")
            .IsUnique();

        modelBuilder.Entity<Session>()
            .Property(s => s.RefreshToken)
            .IsRequired();

        modelBuilder.Entity<Session>()
            .Property(s => s.CreatedAt)
            .HasValueGenerator<CreatedAtGenerator>()
            .IsRequired();

        modelBuilder.Entity<Session>()
            .HasOne(s => s.Account)
            .WithMany()
            .IsRequired();

        modelBuilder.ConfigureAccount();

        modelBuilder.ConfigureWallet();

        modelBuilder.Entity<Holding>()
            .HasKey(h => h.Id);

        modelBuilder.Entity<Holding>()
            .Property(h => h.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Holding>()
            .HasOne(h => h.Account)
            .WithMany(a => a.Holdings)
            .HasForeignKey(h => h.AccountId);

        modelBuilder.ConfigureExternalApiCall();

        modelBuilder.Entity<Account>().HasData(
            new Account {
                Id = Guid.Parse("EB0E7BD5-DF42-46CC-BBE7-7ECB8D8718D9"),
                EmailAddress = "andy@email.com",
                Password = "$2b$10$JKnwr5mA2ux4iN1RXbAVC.92tIwUrjmxiOZfG1DDK/GOtwkPl/7p6",
                FirstName = "Andy",
                LastName = "Sterkowitz",
                CreatedAt = new DateTime(2025, 1, 29, 0, 0, 34)
            }
        );

        modelBuilder.Entity<Wallet>().HasData(
            new Wallet
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.Parse("EB0E7BD5-DF42-46CC-BBE7-7ECB8D8718D9"),
                Amount = 60191
            }    
        );

        modelBuilder.Entity<Holding>().HasData(
            new Holding
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.Parse("EB0E7BD5-DF42-46CC-BBE7-7ECB8D8718D9"),
                Symbol = "TSLA",
                Shares = 100,
                CostBasis = 39809,
                CreatedAt = new DateTime(2025, 1, 29, 0, 2, 54)
            }
        );

        modelBuilder.Entity<Transaction>().HasData(
            new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.Parse("EB0E7BD5-DF42-46CC-BBE7-7ECB8D8718D9"),
                Symbol = "TSLA",
                Type = "BUY",
                Amount = 100,
                Price = 398.09,
                CreatedAt = new DateTime(2025, 1, 29, 0, 2, 54)
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var coreAssemblyDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FantasyStockTraderContext)).Location);
        optionsBuilder
        .EnableSensitiveDataLogging()
        .UseSqlite($"Data Source={Path.Combine(coreAssemblyDirectory, "AppData", "fantasy_stock_trader.db")}", 
            o => o.MigrationsAssembly("FantasyStockTrader.Core"));
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<ExternalApiCall> ExternalApiCalls { get; set; }
}

public class CreatedAtGenerator : ValueGenerator
{
    protected override object? NextValue(EntityEntry entry)
    {
        return DateTime.UtcNow;
    }

    public override bool GeneratesTemporaryValues { get; }
}