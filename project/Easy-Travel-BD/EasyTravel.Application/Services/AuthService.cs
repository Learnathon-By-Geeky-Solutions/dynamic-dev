using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(ILoginService loginService, IRegisterService registerService)
        {
            LoginService = loginService;
            RegisterService = registerService;
        }
        public ILoginService LoginService { get; private set; }

        public IRegisterService RegisterService { get; private set; }
    }
}
