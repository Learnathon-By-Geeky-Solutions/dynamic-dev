using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTravel.Web.Areas.Admin.Models
{
    public class UserViewModel : RegisterViewModel
    {
        public string Role { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
