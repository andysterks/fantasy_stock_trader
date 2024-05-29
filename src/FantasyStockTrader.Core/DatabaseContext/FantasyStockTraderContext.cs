﻿using FantasyStockTrader.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;

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
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        modelBuilder.ConfigureAccount();

        modelBuilder.ConfigureWallet();

        modelBuilder.Entity<Holding>()
            .HasOne(h => h.Account)
            .WithMany(a => a.Holdings)
            .HasForeignKey(h => h.AccountId)
            .IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=../../fantasy_stock_trader.db;foreign keys=true;")
            .LogTo(Console.WriteLine, LogLevel.Trace);

        //optionsBuilder.UseNpgsql(
        //    "Server=localhost; Port=5432; Database=fantasy_stock_trader; User ID=postgres; Password=passw0rd",
        //    optionsBuilder => optionsBuilder.MigrationsAssembly("FantasyStockTrader.Core"));
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