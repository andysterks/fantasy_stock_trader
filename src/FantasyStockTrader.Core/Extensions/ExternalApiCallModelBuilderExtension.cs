using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FantasyStockTrader.Core.Extensions
{
    public static class ExternalApiCallModelBuilderExtension
    {
        public static void ConfigureExternalApiCall(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExternalApiCall>()
                .HasKey(ac => ac.Id);

            modelBuilder.Entity<ExternalApiCall>()
                .HasOne(ac => ac.Account)
                .WithMany()
                .HasForeignKey(ac => ac.AccountId)
                .IsRequired();

            modelBuilder.Entity<ExternalApiCall>()
                .Property(ac => ac.Endpoint)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder.Entity<ExternalApiCall>()
                .Property(ac => ac.IsCached)
                .IsRequired();

            modelBuilder.Entity<ExternalApiCall>()
                .Property(ac => ac.CreatedAt)
                .HasValueGenerator<CreatedAtGenerator>()
                .IsRequired();
        }
    }
}
