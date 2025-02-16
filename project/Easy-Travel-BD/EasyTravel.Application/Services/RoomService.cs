using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class RoomService:IRoomService
    {
        private readonly IApplicationUnitOfWork _applicationunitOfWork;

        public RoomService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationunitOfWork = applicationUnitOfWork;
        }

        public void Create(Room entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Room Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Room> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Room entity)
        {
            throw new NotImplementedException();
        }
    }
}
