using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;

namespace EasyTravel.Web.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterViewModel, User>().ReverseMap();
            CreateMap<SearchFormModel, PhotographerBooking>().ReverseMap();
            CreateMap<BookingFormViewModel, PhotographerBookingViewModel>().ReverseMap();
            CreateMap<PhotographerBookingViewModel, PhotographerBooking>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.PhotographerId, opt => opt.Ignore())
    .ForMember(dest => dest.UserId, opt => opt.Ignore())
    .ForMember(dest => dest.Id, opt => opt.Ignore()) // Assuming it's auto-generated
    .ReverseMap();
            CreateMap<User, BookingFormViewModel>().ReverseMap();
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
