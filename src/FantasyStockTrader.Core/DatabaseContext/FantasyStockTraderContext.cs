using Microsoft.EntityFrameworkCore;

namespace FantasyStockTrader.Core.DatabaseContext
{
    public class FantasyStockTraderContext : DbContext
    {
        public FantasyStockTraderContext(DbContextOptions<FantasyStockTraderContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
