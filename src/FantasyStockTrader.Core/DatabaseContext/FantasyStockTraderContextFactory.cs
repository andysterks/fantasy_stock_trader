using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FantasyStockTrader.Core.DatabaseContext
{
    public class FantasyStockTraderContextFactory : IDesignTimeDbContextFactory<FantasyStockTraderContext>
    {
        private readonly IConfiguration _configuration;

        public FantasyStockTraderContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public FantasyStockTraderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FantasyStockTraderContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("FantasyStockTrader"),
                o => o.MigrationsAssembly("FantasyStockTrader.Core"));
            optionsBuilder.EnableSensitiveDataLogging();

            return new FantasyStockTraderContext(optionsBuilder.Options, _configuration);
        }
    }
}
