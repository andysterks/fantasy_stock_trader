using FantasyStockTrader.Core;

namespace FantasyStockTrader.Web
{
    public interface IAuthContext
    {
        Account Account { get; set; }
    }

    public class AuthContext : IAuthContext
    {
        public Account Account { get; set; }
    }
}
