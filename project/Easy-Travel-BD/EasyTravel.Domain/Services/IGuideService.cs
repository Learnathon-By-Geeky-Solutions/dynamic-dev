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
        Task<(IEnumerable<Guide>,int)> GetGuideListAsync(GuideBooking guideBooking,int pageNumber,int pageSize);
    }
}
