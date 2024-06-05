using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Integration;
using FantasyStockTrader.Web;
using FantasyStockTrader.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddApplicationInsightsTelemetry(o =>
//{
//    o.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
//});

// Add services to the container.

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("FantasyStockTrader");

builder.Services.AddDbContext<FantasyStockTraderContext>(
    options => options.UseSqlServer(connectionString,
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
builder.Services.AddScoped<IFinnhubApiService, FinnhubApiService>();
builder.Services.AddScoped<IHoldingsUpdateService, HoldingsUpdateService>();
builder.Services.AddScoped<IWalletUpdateService, WalletUpdateService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBuyStockService, BuyStockService>();
builder.Services.AddScoped<IBuySummaryService, BuySummaryService>();
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

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
