using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FantasyStockTrader.Core.DatabaseContext
{
    public class FantasyStockTraderContextFactory : IDesignTimeDbContextFactory<FantasyStockTraderContext>
    {
        public FantasyStockTraderContextFactory()
        {
        }

        public FantasyStockTraderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FantasyStockTraderContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=fantasy_stock_trader;Trusted_Connection=True;TrustServerCertificate=True;",
                o => o.MigrationsAssembly("FantasyStockTrader.Core"));
            optionsBuilder.EnableSensitiveDataLogging();

            return new FantasyStockTraderContext(optionsBuilder.Options);
        }
    }
}
