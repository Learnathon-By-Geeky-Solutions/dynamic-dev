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
using EasyTravel.Web.Hubs;
using Microsoft.AspNetCore.Identity;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Middleware;
using Autofac.Core;
using EasyTravel.Infrastructure.BackgroundServices;


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
        containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly.FullName!));
        // Register HttpClient using IHttpClientFactory
        containerBuilder.Register(ctx =>
        {
            var factory = ctx.Resolve<IHttpClientFactory>();
            return factory.CreateClient();
        }).As<HttpClient>().InstancePerLifetimeScope();
    });
    #endregion




    builder.Host.UseSerilog((context, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration));

    #region Docker IP Correction
    builder.WebHost.UseUrls("http://*:80");
    #endregion


    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentity<User,Role>(
        options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 0;
        }
        )
    .AddEntityFrameworkStores<ApplicationDbContext>();




    builder.Services.AddAutoMapper(migrationAssembly);

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
        options.Cookie.HttpOnly = true;                // Make cookies accessible only through HTTP
        options.Cookie.IsEssential = true;             // Essential for GDPR compliance
    });
    builder.Services.AddHostedService<DatabaseCleanUpService>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSignalR();
    // Register HttpClient in the default DI container
    builder.Services.AddHttpClient();
    var app = builder.Build();


    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await IdentitySeedData.SeedAdminUserAsync(services);
    }



    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseMiddleware<NoCacheMiddleware>();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseSession();

    // Add this in your Program.cs where you configure app
    app.MapHub<SeatHub>("/seatHub");

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseStaticFiles();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id1?}/{id2?}")
        .WithStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id1?}/{id2?}")
        .WithStaticAssets();

    await app.RunAsync();

}


catch (Exception ex)
{

    Log.Fatal(ex, "Application Crashed");

}
finally
{
    await Log.CloseAndFlushAsync();

}

