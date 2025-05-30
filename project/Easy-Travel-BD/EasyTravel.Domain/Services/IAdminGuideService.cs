﻿using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminGuideService : IService<Guide,Guid>
    {
        Guide GetGuideInstance();
        Task<PagedResult<Guide>> GetPaginatedGuidesAsync(int pageNumber, int pageSize);
    }
}
