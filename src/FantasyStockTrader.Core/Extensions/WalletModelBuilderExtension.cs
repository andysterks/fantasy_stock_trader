using Microsoft.EntityFrameworkCore;

namespace FantasyStockTrader.Core.Extensions
{
    public static class WalletModelBuilderExtension
    {
        public static void ConfigureWallet(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<Wallet>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Wallet>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(19,4)");

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.Account)
                .WithOne()
                .HasForeignKey<Wallet>(w => w.AccountId)
                .IsRequired();
        }
    }
}
