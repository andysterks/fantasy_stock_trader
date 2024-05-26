namespace FantasyStockTrader.Web
{
    public static class ServiceProviderExtensions
    {
        public static IHttpContextAccessor GetHttpContextAccessor(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IHttpContextAccessor>();
        }
    }
}
