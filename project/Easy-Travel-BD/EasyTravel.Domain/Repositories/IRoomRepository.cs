﻿using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Repositories
{
    public interface IRoomRepository : IRepository<Room, Guid>
    {
      public  IEnumerable<Room> GetRooms(Guid id);
    }
}
