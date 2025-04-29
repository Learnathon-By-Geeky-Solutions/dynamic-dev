using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IPhotographerService : IGetService<Photographer,Guid>
    {
        Task<(IEnumerable<Photographer>, int)> GetPhotographerListAsync(PhotographerBooking photographerBooking,int pageNumber,int pageSize);
    }
}