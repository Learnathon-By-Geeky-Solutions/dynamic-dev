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
            CreateMap<BookingFormViewModel, PhotographerBookingViewModel >()
                .ForMember(dest => dest.UserName,opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<PhotographerBookingViewModel, PhotographerBooking>().ReverseMap();
        }
    }
}
