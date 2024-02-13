using Microsoft.EntityFrameworkCore;

namespace FantasyStockTrader.Core.DatabaseContext
{
    public class FantasyStockTraderContextFactory
    {
        public FantasyStockTraderContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FantasyStockTraderContext>();
            optionsBuilder.UseNpgsql(x => x.MigrationsAssembly("FantasyStockTrader.Core"));
            optionsBuilder.EnableSensitiveDataLogging();

            return new FantasyStockTraderContext(optionsBuilder.Options);
        }
    }
}
