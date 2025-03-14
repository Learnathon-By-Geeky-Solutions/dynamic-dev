using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;

namespace EasyTravel.Web.Mappings
{
    public class UserMappingProfile:Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterViewModel, User>();
        }
    }
}
