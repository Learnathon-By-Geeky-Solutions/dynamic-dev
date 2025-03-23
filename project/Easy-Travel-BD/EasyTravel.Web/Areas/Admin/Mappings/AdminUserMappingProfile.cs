using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Areas.Admin.Models;
using EasyTravel.Web.Models;

namespace EasyTravel.Web.Areas.Admin.Mappings
{
    public class AdminUserMappingProfile : Profile
    {
        public AdminUserMappingProfile()
        {
            CreateMap<User,AdminUserViewModel>().ReverseMap();
        }
    }
}
