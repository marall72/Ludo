using Ludo.Business;
using Ludo.Database;
using Ludo.Middleware;
using Ludo.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LudoDbContext>(options =>
 
{
    //don't do this if not in development
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(message => Debug.WriteLine(message), LogLevel.Information);
});
builder.Services.AddScoped<UserBusiness>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.MapStaticAssets();
app.UseMiddleware<AuthMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
