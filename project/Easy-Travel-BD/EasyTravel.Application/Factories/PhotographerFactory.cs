using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Factories
{
    public class PhotographerFactory : IEntityFactory<Photographer>
    {
        public Photographer CreateInstance()
        {
            return new Photographer
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                ContactNumber = string.Empty,
                Address = string.Empty,
                ProfilePicture = string.Empty,
                Bio = string.Empty,
                DateOfBirth = DateTime.MinValue,
                Skills = string.Empty,
                PortfolioUrl = string.Empty,
                Specialization = string.Empty,
                YearsOfExperience = 0,
                Availability = false,
                HourlyRate = 0,
                Rating = 0,
                SocialMediaLinks = null,
                Status = null,
                AgencyId = default
            };
        }
    }
}
