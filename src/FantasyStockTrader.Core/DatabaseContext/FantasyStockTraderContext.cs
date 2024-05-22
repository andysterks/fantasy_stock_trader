using Microsoft.EntityFrameworkCore;

namespace FantasyStockTrader.Core.DatabaseContext
{
    public class FantasyStockTraderContext : DbContext
    {
        public FantasyStockTraderContext(DbContextOptions<FantasyStockTraderContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            modelBuilder.Entity<Account>();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Holding> Holdings { get; set; }
    }
}
