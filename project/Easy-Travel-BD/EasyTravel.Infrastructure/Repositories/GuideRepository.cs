using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.Repositories
{
    public class GuideRepository : Repository<Guide,Guid>,IGuideRepository
    {
        private readonly ApplicationDbContext _context;
        public GuideRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
