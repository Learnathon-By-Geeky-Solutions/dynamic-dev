using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;

namespace EasyTravel.Web.Mappings
{
    public class UserMappingProfile:Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterViewModel, User>().ReverseMap();
            CreateMap<SearchFormModel, PhotographerBooking>().ReverseMap();
            CreateMap<BookingFormViewModel, PhotographerBookingViewModel>().ReverseMap();
            CreateMap<User, BookingFormViewModel>().ReverseMap();
            CreateMap<Photographer,PhotographerBookingViewModel>()
                .ForMember(dest => dest.PhotographerPhoneNumber,opt=> opt.MapFrom(src => src.ContactNumber))
                .ForMember(dest => dest.PhotographerFirstName,opt=> opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.PhotographerLastName,opt=> opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhotographerEmail,opt=> opt.MapFrom(src => src.Email))
                .ReverseMap();
            CreateMap<BookingFormViewModel, GuideBookingViewModel>().ReverseMap();
            CreateMap<User, BookingFormViewModel>().ReverseMap();
            CreateMap<Guide, GuideBookingViewModel>()
                .ForMember(dest => dest.GuidePhoneNumber, opt => opt.MapFrom(src => src.ContactNumber))
                .ForMember(dest => dest.GuideFirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.GuideLastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.GuideEmail, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }
}
