using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class BusSearchController : Controller
    {
        private ISessionService _sessionService;
        private IBusService _busService;

        public BusSearchController( IBusService busservice ,ISessionService sessionService)
        {
            _sessionService = sessionService;
            _busService = busservice;
        }

      
    }

}

