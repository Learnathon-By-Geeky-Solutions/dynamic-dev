using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Areas.Admin.Models
{
    public class AdminUserRoleModel
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public List<User>? Users { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
