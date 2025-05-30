﻿using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Repositories
{
    public interface IRecommendationRepository
    {
        public Task<IEnumerable<RecommendationDto>> GetRecommendationsAsync(string type, int count);
    }
}
