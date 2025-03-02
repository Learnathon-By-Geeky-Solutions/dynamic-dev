using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class HotelRoomViewModel
    {
        public Hotel Hotel { get; set; }
        public List<Room> Rooms { get; set; }= new List<Room>();
    }
}
