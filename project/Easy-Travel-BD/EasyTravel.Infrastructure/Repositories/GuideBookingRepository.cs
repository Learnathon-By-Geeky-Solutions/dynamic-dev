﻿using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.Repositories
{
    public class GuideBookingRepository : Repository<GuideBooking,Guid>,IGuideBookingRepository
    {
        public GuideBookingRepository(ApplicationDbContext context) : base(context) 
        {
        }
    }
}
