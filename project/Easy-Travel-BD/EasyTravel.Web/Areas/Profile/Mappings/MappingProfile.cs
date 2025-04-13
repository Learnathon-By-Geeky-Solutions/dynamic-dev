
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Areas.Profile.Models;

namespace EasyTravel.Web.Areas.Profile.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ProfileUserModel, User>().ReverseMap();
        }
    }
}
