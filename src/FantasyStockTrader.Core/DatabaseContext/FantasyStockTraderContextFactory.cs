using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
            //optionsBuilder.UseNpgsql(x => x.MigrationsAssembly("FantasyStockTrader.Core"));
            optionsBuilder.UseSqlite(x => x.MigrationsAssembly("FantasyStockTrader.Core"))
                .LogTo(Console.WriteLine, LogLevel.Trace);
            optionsBuilder.EnableSensitiveDataLogging();

            return new FantasyStockTraderContext(optionsBuilder.Options);
        }
    }
}
