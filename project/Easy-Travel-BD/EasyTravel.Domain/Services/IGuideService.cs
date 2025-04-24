using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IGuideService : IGetService<Guide, Guid>
    {
        Task<IEnumerable<Guide>> GetGuideListAsync(GuideBooking guideBooking);
    }
}
