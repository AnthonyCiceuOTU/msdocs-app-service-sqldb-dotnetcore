using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using DotNetCoreSqlDb.Helpers;
using Microsoft.AspNetCore.Authorization;
using DotNetCoreSqlDb.Models.Config;
using DotNetCoreSqlDb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<LogHelper>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<MyDatabaseContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection")));

    builder.Services.AddDistributedMemoryCache();
}
else
{
    builder.Services.AddDbContext<MyDatabaseContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["AZURE_REDIS_CONNECTIONSTRING"];
        options.InstanceName = "SampleInstance";
    });
}

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("FullUserOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(context =>
            context.User.FindFirst("IsGuest")?.Value != "true");
    });
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Logging.AddAzureWebAppDiagnostics();

builder.Services.Configure<GeminiOptions>(options =>
{
    options.ApiKey = builder.Configuration["GeminiAPIKey"];
    options.Model = "gemini-2.5-flash";
});

builder.Services.AddScoped<IAiShortAnswerGrader, GeminiShortAnswerGrader>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddAzureWebAppDiagnostics();

Console.WriteLine("GeminiAPIKey exists in configuration: " +
    !string.IsNullOrWhiteSpace(builder.Configuration["GeminiAPIKey"]));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();