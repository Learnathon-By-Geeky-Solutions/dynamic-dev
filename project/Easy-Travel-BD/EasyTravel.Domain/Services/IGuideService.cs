using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IGuideService : IGetService<Guide, Guid>
    {
        Task<PagedResult<Guide>> GetGuideListAsync(GuideBooking guideBooking,int pageNumber,int paseSize);
    }
}
