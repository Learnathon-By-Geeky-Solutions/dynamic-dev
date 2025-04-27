using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminPhotographerService : IService<Photographer,Guid>
    {
        public Photographer GetPhotographerInstance();
        Task<PagedResult<Photographer>> GetPaginatedPhotographersAsync(int pageNumber, int pageSize);
    }
}
