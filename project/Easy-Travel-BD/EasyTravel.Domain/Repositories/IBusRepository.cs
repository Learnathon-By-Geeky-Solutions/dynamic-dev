﻿using EasyTravel.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Repositories
{
    public interface IBusRepository:IRepository<Bus,Guid>
    {
        void Addbus(Bus bus);
        IEnumerable<Bus> GetAllBuses();

        public IQueryable<Bus> GetBuses();
    }
}