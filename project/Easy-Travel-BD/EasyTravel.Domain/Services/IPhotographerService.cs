using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IPhotographerService
    {
        void AddPhotographer(Photographer Photographer);
        void UpdatePhotographer(Photographer Photographer);
        void DeletePhotographer(Guid id);
        Photographer GetPhotographerById(Guid id);
        IEnumerable<Photographer> GetAllPhotographers();

        Photographer CreatePhographerInstance();
    }
}
