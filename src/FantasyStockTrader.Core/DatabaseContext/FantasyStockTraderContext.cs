using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;

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

        modelBuilder.Entity<Account>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Account>()
            .Property(a => a.EmailAddress)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.EmailAddress)
            .HasDatabaseName("UXC_Account_EmailAddress")
            .IsUnique();

        modelBuilder.Entity<Account>()
            .Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Account>()
            .Property(a => a.Password)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Account>()
            .Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(75);

        modelBuilder.Entity<Account>()
            .Property(a => a.CreatedAt)
            .HasValueGenerator<CreatedAtGenerator>()
            .IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Server=localhost; Port=5432; Database=fantasy_stock_trader; User ID=postgres; Password=passw0rd",
            optionsBuilder => optionsBuilder.MigrationsAssembly("FantasyStockTrader.Core"));
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Holding> Holdings { get; set; }
}

public class CreatedAtGenerator : ValueGenerator
{
    protected override object? NextValue(EntityEntry entry)
    {
        return DateTime.UtcNow;
    }

    public override bool GeneratesTemporaryValues { get; }
}