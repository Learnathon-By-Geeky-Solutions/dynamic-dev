using EasyTravel.Domain;
using EasyTravel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _dbContext;


        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
        }




        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
