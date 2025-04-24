using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.Repositories
{
    public class PhotographerRepository : Repository<Photographer, Guid>, IPhotographerRepository
    {
        public PhotographerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
