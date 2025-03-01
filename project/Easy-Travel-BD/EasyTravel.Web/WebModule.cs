using Autofac;
using EasyTravel.Application.Factories;
using EasyTravel.Application.Services;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using EasyTravel.Infrastructure;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using System.ComponentModel;

namespace EasyTravel.Web
{
    public class WebModule: Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;


        public WebModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
             .WithParameter("connectionString", _connectionString)
            .WithParameter("migrationAssembly", _migrationAssembly)
            .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>()
             .InstancePerLifetimeScope();
            builder.RegisterType<BusRepository>().As<IBusRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<AgencyRepository>().As<IAgencyRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerRepository>().As<IPhotographerRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<GuideRepository>().As<IGuideRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<BusService>().As<IBusService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<AgencyService>().As<IAdminAgencyService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerService>().As<IAdminPhotographerService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<GuideService>().As<IAdminGuideService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<SessionService>().As<ISessionService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerFactory>().As<IPhotographerFactory>()
            .InstancePerLifetimeScope();
            builder.RegisterType<GuideFactory>().As<IGuideFactory>()
            .InstancePerLifetimeScope();
            base.Load(builder);
        }

    }
}
