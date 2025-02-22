using Microsoft.EntityFrameworkCore;

using EasyTravel.Infrastructure.Data;
using EasyTravel.Application.Services;
using EasyTravel.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using EasyTravel.Web;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.Repositories;
using System.Reflection;


var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();


Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateBootstrapLogger();


try
{


    Log.Information("Application Starting...... ");

    var builder = WebApplication.CreateBuilder(args);




    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    var migrationAssembly = Assembly.GetExecutingAssembly();

    #region autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly?.FullName));
    });
    #endregion




    builder.Host.UseSerilog((context, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)


    );

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();




    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {

        options.IdleTimeout = TimeSpan.FromMinutes(60); // Set session timeout
        options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
        options.Cookie.HttpOnly = true;                // Make cookies accessible only through HTTP
        options.Cookie.IsEssential = true;             // Essential for GDPR compliance
    });
    builder.Services.AddHttpContextAccessor();

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
    app.UseSession();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


    app.Run();

}


catch (Exception ex)
{

    Log.Fatal(ex, "Application Crashed");

}
finally
{
    Log.CloseAndFlush();

}

