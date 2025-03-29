using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTravel.Web.Areas.Admin.Models
{
    public class AdminUserViewModel : RegisterViewModel
    {
        public Guid Id { get; set; }
    }
}
