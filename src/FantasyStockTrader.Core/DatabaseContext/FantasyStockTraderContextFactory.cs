using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FantasyStockTrader.Core.DatabaseContext
{
    public class FantasyStockTraderContextFactory : IDesignTimeDbContextFactory<FantasyStockTraderContext>
    {
        private readonly IConfiguration _configuration;

        public FantasyStockTraderContextFactory() { }

        public FantasyStockTraderContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public FantasyStockTraderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FantasyStockTraderContext>();
            var coreAssemblyDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FantasyStockTraderContext)).Location);
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(coreAssemblyDirectory, "AppData", "fantasy_stock_trader.db")}",
                o => o.MigrationsAssembly("FantasyStockTrader.Core"));
            optionsBuilder.EnableSensitiveDataLogging();

            return new FantasyStockTraderContext(optionsBuilder.Options);
        }
    }
}
