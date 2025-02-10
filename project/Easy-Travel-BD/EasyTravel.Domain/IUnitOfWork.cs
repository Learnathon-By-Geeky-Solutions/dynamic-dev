using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain
{
    public interface IUnitOfWork
    {
        void Save();
    }
}
