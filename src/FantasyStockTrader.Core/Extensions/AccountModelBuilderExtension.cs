using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FantasyStockTrader.Core.Extensions
{
    public static class AccountModelBuilderExtension
    {
        public static void ConfigureAccount(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Account>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();

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
    }
}
