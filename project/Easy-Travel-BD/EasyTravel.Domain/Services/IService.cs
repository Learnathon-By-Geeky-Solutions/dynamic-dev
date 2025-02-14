using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IService<TEntity>
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(Guid id);
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
    }
}
