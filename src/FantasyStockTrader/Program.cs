using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Core.Exceptions;
using FantasyStockTrader.Web;
using FantasyStockTrader.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("FantasyStockTrader");

builder.Services.AddDbContext<FantasyStockTraderContext>(
    options => options.UseNpgsql(connectionString,
        optionsBuilder => optionsBuilder.MigrationsAssembly("FantasyStockTrader.Core")));

builder.Services.AddAuthentication("FSTScheme")
    .AddScheme<CookieAuthenticationOptions, CookieAuthenticationHandler>("FSTScheme", null);

builder.Services.AddScoped((sp) => sp.GetRequiredService<IAuthUserGenerationService>().GetAuthContext());

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthUserGenerationService, AuthUserGenerationService>();
builder.Services.AddScoped<IAuthCookieService, AuthCookieService>();
builder.Services.AddScoped<IAuthTokenCreationService, AuthTokenCreationService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRefreshTokenRenewalService, RefreshTokenRenewalService>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//app.UseMiddleware<TokenMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthTokenCreationService _authTokenCreationService;
    private readonly IAuthCookieService _authCookieService;

    public TokenMiddleware(RequestDelegate next, 
        IAuthTokenCreationService authTokenCreationService, 
        IAuthCookieService authCookieService)
    {
        _next = next;
        _authTokenCreationService = authTokenCreationService;
        _authCookieService = authCookieService;
    }

    public async Task Invoke(HttpContext context)
    {
        var accessTokenCookie = context.Request.Cookies["fst-access-id"];
        if (accessTokenCookie == null)
        {
            var refreshTokenCookie = context.Request.Cookies["fst-refresh-id"];
            if (refreshTokenCookie != null)
            {
                
            }
        }
        await _next(context);


    }
}
