using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTravel.Web.Areas.Admin.Models
{
    public class AdminUserViewModel : RegisterViewModel
    {
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
