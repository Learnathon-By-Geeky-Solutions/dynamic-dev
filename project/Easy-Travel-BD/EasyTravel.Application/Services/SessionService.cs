using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetString(string key)
        {
            if (_httpContextAccessor.HttpContext.Session.TryGetValue(key, out byte[] bytes) == true)
            {
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            return null; 
        }

        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }

        public void SetString(string key, string value)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
            _httpContextAccessor.HttpContext?.Session.Set(key,bytes);
        }
    }
}
