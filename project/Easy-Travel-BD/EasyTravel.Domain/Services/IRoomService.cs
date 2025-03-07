﻿using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IRoomService:IService<Room,Guid>
    {
        public IEnumerable<Room> GetRoomByHotel(Guid id);
    }
}
