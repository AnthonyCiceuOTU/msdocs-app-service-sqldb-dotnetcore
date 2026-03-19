﻿using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Azure.Identity;
//using DotNetCoreSqlDb.Services;
//using DotNetCoreSqlDb.Settings;
//using Azure.Extensions.AspNetCore.Configuration.Secrets;
using System.Net.Http.Headers;
//using DotNetCoreSqlDb.Hubs;
using DotNetCoreSqlDb.Helpers;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<LogHelper>();
// Add database context and cache
if(builder.Environment.IsDevelopment())
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

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath          = "/Login/Login";
        options.LogoutPath         = "/Login/Logout";
        options.AccessDeniedPath   = "/Home/AccessDenied";
        options.ExpireTimeSpan     = TimeSpan.FromHours(24);
        options.SlidingExpiration  = true;
    });


// Add App Service logging
builder.Logging.AddAzureWebAppDiagnostics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
