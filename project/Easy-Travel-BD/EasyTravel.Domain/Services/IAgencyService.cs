using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAgencyService
    {
        void AddAgency(Agency agency);
        void UpdateAgency(Agency agency);
        void DeleteAgency(Agency agency);
    }
}
