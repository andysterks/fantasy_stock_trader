using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FantasyStockTrader.IntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FantasyStockTraderContext>();
            var coreAssembly = Assembly.GetAssembly(typeof(FantasyStockTraderContext));
            var coreAssemblyDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FantasyStockTraderContext)).Location);
            var dataSource = $"Data Source={Path.Combine(coreAssemblyDirectory, "AppData", "fantasy_stock_trader.db")}";
            optionsBuilder.UseSqlite(dataSource);
            optionsBuilder.EnableSensitiveDataLogging();

            var dbContext = new FantasyStockTraderContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();

            dbContext.Accounts.Add(new Core.Account
            {
                EmailAddress = "andy@email.com",
                FirstName = "andy",
                LastName = "s",
                Password = "1234"
            });
            dbContext.SaveChanges();
        }
    }
}