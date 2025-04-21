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
            builder.RegisterType<AdminGuideBookingService>().As<IAdminGuideBookingService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<AdminPhotographerBookingService>().As<IAdminPhotographerBookingService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AdminHotelBookingService>().As<IAdminHotelBookingService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AdminBusBookingService>().As<IAdminBusBookingService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AdminCarBookingService>().As<IAdminCarBookingService>()
             .InstancePerLifetimeScope();
            builder.RegisterType<BookingHistoryService>().As<IBookingHistoryService>()
             .InstancePerLifetimeScope();
            builder.RegisterType<BookingRepository>().As<IBookingRepository>()
             .InstancePerLifetimeScope();
            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>()
             .InstancePerLifetimeScope();
            builder.RegisterType<PaymentOnlyService>().As<IPaymentOnlyService>()
             .InstancePerLifetimeScope();
            builder.RegisterType<PhotographerPaymentService>().As<IPaymentBookingService<PhotographerBooking,Booking, Guid>>()
             .InstancePerLifetimeScope();
            builder.RegisterType<BookingService>().As<IGetService<Booking, Guid>>()
             .InstancePerLifetimeScope();
            builder.RegisterType<GuidePaymentService>().As<IPaymentBookingService<GuideBooking, Booking, Guid>>()
             .InstancePerLifetimeScope();
            builder.RegisterType<CarPaymentService>().As<IPaymentBookingService<CarBooking, Booking, Guid>>()
             .InstancePerLifetimeScope();
            builder.RegisterType<BusPaymentService>().As<IPaymentBookingService<BusBooking, Booking, Guid>>()
             .InstancePerLifetimeScope();
            builder.RegisterType<HotelPaymentService>().As<IPaymentBookingService<HotelBooking, Booking, Guid>>()
             .InstancePerLifetimeScope();
            base.Load(builder);
        }

    }
}
