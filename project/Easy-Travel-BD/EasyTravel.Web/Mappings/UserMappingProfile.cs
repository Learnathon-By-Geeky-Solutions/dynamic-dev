using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Areas.Admin.Models;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;

namespace EasyTravel.Web.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AdminUserViewModel, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();
            CreateMap<RegisterViewModel, User>().ReverseMap();
            CreateMap<SearchFormModel, PhotographerBooking>().ReverseMap();
            CreateMap<SearchFormModel, GuideBooking>().ReverseMap();
            CreateMap<BookingFormViewModel, PhotographerBookingViewModel>().ReverseMap();
            CreateMap<PhotographerBookingViewModel, PhotographerBooking>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
        //.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.PhotographerId, opt => opt.Ignore())
        //.ForMember(dest => dest.UserId, opt => opt.Ignore())
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ReverseMap();
            CreateMap<User, BookingFormViewModel>().ReverseMap();
            CreateMap<User, PhotographerViewModel>().ReverseMap();
            CreateMap<User, GuideBookingViewModel>().ReverseMap();
            CreateMap<User, PhotographerBookingViewModel>().ReverseMap();
            CreateMap<BookingFormViewModel, GuideBookingViewModel>().ReverseMap();
            CreateMap<GuideBookingViewModel, GuideBooking>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
          //.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
          .ForMember(dest => dest.GuideId, opt => opt.Ignore())
          //.ForMember(dest => dest.UserId, opt => opt.Ignore())
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ReverseMap();
        }
    }
}
