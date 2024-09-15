using System.Net;
using FantasyStockTrader.Core.Exceptions;
using Newtonsoft.Json;

namespace FantasyStockTrader.Web
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is FSTAuthorizationException)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    var error = new ErrorDTO(ex.Message);
                    var json = JsonConvert.SerializeObject(error);
                    await httpContext.Response.WriteAsync(json);
                }
            }
        }
    }

    public record ErrorDTO(string message);
}
