using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Factories
{
    public class GuideFactory : IEntityFactory<Guide>
    {
        public Guide CreateInstance()
        {
            return new Guide
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                ContactNumber = string.Empty,
                Address = string.Empty,
                ProfilePicture = string.Empty,
                Bio = string.Empty,
                DateOfBirth = DateTime.MinValue,
                LanguagesSpoken = string.Empty,
                LicenseNumber = string.Empty,
                Specialization = string.Empty,
                YearsOfExperience = 0,
                Availability = false,
                HourlyRate = 0,
                Rating = 0,
                Status = null,
                AgencyId = default
            };
        }
    }
}
