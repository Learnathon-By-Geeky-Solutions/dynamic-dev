﻿using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IBookingService<TEntity,TEntity2>
    {
        void AddBooking(TEntity model,TEntity2 bookingmodel);
        bool CancelBooking();
        Task<bool> IsBooked(TEntity model);
        public Task<IEnumerable<TEntity>> GetBookingListByFormDataAsync(TEntity model);
    }
}
