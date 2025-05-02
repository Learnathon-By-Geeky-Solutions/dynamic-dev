using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Infrastructure.Data;

namespace EasyTravel.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room, Guid>, IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Room> GetRooms(Guid id)
        {
            return _context.Rooms.Where(r => r.HotelId == id);
        }

    }
}
