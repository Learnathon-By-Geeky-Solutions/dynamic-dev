using Autofac;
using EasyTravel.Application.Services;
using EasyTravel.Domain;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using EasyTravel.Infrastructure;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using System.ComponentModel;

namespace Easy_Travell_BD
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
            builder.RegisterType<BusService>().As<IBusService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>()
            .InstancePerLifetimeScope();



            base.Load(builder);
        }

    }
}
