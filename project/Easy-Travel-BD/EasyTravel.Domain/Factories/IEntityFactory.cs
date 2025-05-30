﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Factories
{
    public interface IEntityFactory<out TEntity>
    {
        TEntity CreateInstance();
    }
}
