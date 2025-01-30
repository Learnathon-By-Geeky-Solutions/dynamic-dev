using Microsoft.EntityFrameworkCore;

using EasyTravel.Infrastructure.Data;
using EasyTravel.Application.Services.Interfaces;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Interfaces;
using EasyTravel.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Easy_Travell_BD;


var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();


Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateBootstrapLogger();


try
{


    Log.Information("Application Starting.... ");

    var builder = WebApplication.CreateBuilder(args);




    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");



    #region autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(ContainerBuilder =>
    {

        ContainerBuilder.RegisterModule( new WebModule());



    });
    #endregion




    builder.Host.UseSerilog((context, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)


    );

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();


    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IBusRepository, BusRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IBusService, BusService>();



    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
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
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


    app.Run();

}


catch(Exception ex)
{

    Log.Fatal(ex, "Application Crashed");

}
finally
{
    Log.CloseAndFlush();

}

