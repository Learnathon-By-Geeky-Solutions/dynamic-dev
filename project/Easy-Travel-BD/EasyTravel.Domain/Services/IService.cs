using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IService<TEntity,Tkey> : IGetService<TEntity, Tkey>
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(Tkey id);
    }
}
