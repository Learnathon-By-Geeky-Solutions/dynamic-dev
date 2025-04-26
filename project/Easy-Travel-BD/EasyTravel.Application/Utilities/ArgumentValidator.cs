using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Utilities
{
    public static class ArgumentValidator
    {
        public static void ThrowIfNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
