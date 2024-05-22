using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FantasyStockTrader.Core.DatabaseContext
{
    public class FantasyStockTraderContextFactory : IDesignTimeDbContextFactory<FantasyStockTraderContext>
    {
        public FantasyStockTraderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FantasyStockTraderContext>();
            optionsBuilder.UseNpgsql(x => x.MigrationsAssembly("FantasyStockTrader.Core"));
            optionsBuilder.EnableSensitiveDataLogging();

            return new FantasyStockTraderContext(optionsBuilder.Options);
        }
    }
}
