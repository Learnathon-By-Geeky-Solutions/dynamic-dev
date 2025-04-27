using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminAgencyService : IService<Agency,Guid>
    {
        Task<PagedResult<Agency>> GetPaginatedAgenciesAsync(int pageNumber, int pageSize);
        Agency CreateInstance();
    }
}
