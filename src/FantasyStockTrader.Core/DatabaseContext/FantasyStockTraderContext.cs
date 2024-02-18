using FantasyStockTrader.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;

namespace FantasyStockTrader.Core.DatabaseContext;

public class FantasyStockTraderContext : DbContext
{
    private IConfiguration _configuration;

    public FantasyStockTraderContext(DbContextOptions<FantasyStockTraderContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
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
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("FantasyStockTrader"),
            o => o.MigrationsAssembly("FantasyStockTrader.Core"));
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