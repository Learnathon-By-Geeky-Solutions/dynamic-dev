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
            builder.RegisterType<CarRepository>().As<ICarRepository>()
             .InstancePerLifetimeScope();
            builder.RegisterType<BusBookingRepository>().As<IBusBookingRepository>()
           .InstancePerLifetimeScope();
            builder.RegisterType<SeatRepository>().As<ISeatRepository>()
           .InstancePerLifetimeScope();

            builder.RegisterType<CarBookingRepository>().As<ICarBookingRepository>()
         .InstancePerLifetimeScope();

            builder.RegisterType<AgencyRepository>().As<IAgencyRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerRepository>().As<IPhotographerRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<GuideRepository>().As<IGuideRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerBookingRepository>().As<IPhotographerBookingRepository>()
            .InstancePerLifetimeScope();
            builder.RegisterType<GuideBookingRepository>().As<IGuideBookingRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BusService>().As<IBusService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<CarService>().As<ICarService>()
           .InstancePerLifetimeScope();
            builder.RegisterType<AdminUserService>().As<IAdminUserService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<AdminAgencyService>().As<IAdminAgencyService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<AdminPhotographerService>().As<IAdminPhotographerService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<AdminGuideService>().As<IAdminGuideService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<SessionService>().As<ISessionService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<PhotographerFactory>().As<IPhotographerFactory>()
            .InstancePerLifetimeScope();
            builder.RegisterType<GuideFactory>().As<IGuideFactory>()
            .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerService>().As<IPhotographerService>()
            .InstancePerLifetimeScope();
            builder.RegisterType<GuideService>().As<IGuideService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<HotelRepository>().As<IHotelRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<HotelService>().As<IHotelService>()
             .InstancePerLifetimeScope();
            builder.RegisterType<RoomRepository>().As<IRoomRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<RoomService>().As<IRoomService>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<HotelBookingRepository>().As<IHotelBookingRepository>()
                 .InstancePerLifetimeScope();
            builder.RegisterType<HotelBookingService>().As<IHotelBookingService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<AuthService>().As<IAuthService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<LoginService>().As<ILoginService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<RegisterService>().As<IRegisterService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AdminRoleService>().As<IAdminRoleService>()
               .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerBookingService>().As<IPhotographerBookingService>()
              .InstancePerLifetimeScope();
            builder.RegisterType<GuideBookingService>().As<IGuideBookingService>()
              .InstancePerLifetimeScope();
            builder.RegisterType<AgencyService>().As<IAgencyService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AdminUserRoleService>().As<IAdminUserRoleService>()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }

    }
}
