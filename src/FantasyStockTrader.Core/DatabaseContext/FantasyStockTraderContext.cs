using FantasyStockTrader.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace FantasyStockTrader.Core.DatabaseContext;

public class FantasyStockTraderContext : DbContext
{
    public FantasyStockTraderContext(DbContextOptions<FantasyStockTraderContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();

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
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        modelBuilder.ConfigureAccount();

        modelBuilder.ConfigureWallet();

        modelBuilder.Entity<Transaction>()
            .HasOne(h => h.Account)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        modelBuilder.Entity<Holding>()
            .HasOne(h => h.Account)
            .WithMany(a => a.Holdings)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get the current working directory
        string currentDirectory = Directory.GetCurrentDirectory();

        // Combine the current directory with the relative path to the database file
        string databasePath = Path.Combine(currentDirectory, "AppData", "fantasy_stock_trader.db");

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(databasePath));

        // Configure the SQLite connection
        optionsBuilder.UseSqlite($"Data Source=\"C:\\Users\\andys\\source\\sqlite_databases\\fantasy_stock_trader.db\";Pooling=False;Cache=Shared;")
                        .AddInterceptors(new SqlCommandInterceptor())
                      .LogTo(Console.WriteLine, LogLevel.Information)
                      .EnableDetailedErrors()
                      .EnableSensitiveDataLogging();
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
}

public class CreatedAtGenerator : ValueGenerator
{
    protected override object? NextValue(EntityEntry entry)
    {
        return DateTime.UtcNow;
    }

    public override bool GeneratesTemporaryValues { get; }
}

public class SqlCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
    {
        Console.WriteLine($"Command Text: {eventData.CommandId}");
        return base.CommandCreating(eventData, result);
    }
}