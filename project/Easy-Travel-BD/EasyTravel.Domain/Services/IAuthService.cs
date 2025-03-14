using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAuthService
    {
        public ILoginService LoginService { get; }
        public IRegisterService RegisterService { get; }
    }
}
