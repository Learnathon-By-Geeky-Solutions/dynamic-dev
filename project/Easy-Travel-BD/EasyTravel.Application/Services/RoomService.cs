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
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public RoomService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public void Create(Room entity)
        {
            _applicationUnitOfWork.RoomRepository.Add(entity);
            _applicationUnitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            _applicationUnitOfWork.RoomRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Room Get(Guid id)
        {
            return _applicationUnitOfWork.RoomRepository.GetById(id);
        }

        public IEnumerable<Room> GetAll()
        {
            return _applicationUnitOfWork.RoomRepository.GetAll();
        }

        public void Update(Room entity)
        {
            _applicationUnitOfWork.RoomRepository.Edit(entity);
            _applicationUnitOfWork.Save();
        }
    }
}
