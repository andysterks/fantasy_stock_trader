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
                .HasOne(w => w.Account)
                .WithMany()
                .HasForeignKey(w => w.Account.Id)
                .IsRequired();
        }
    }
}
