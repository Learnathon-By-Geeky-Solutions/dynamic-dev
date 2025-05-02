using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace EasyTravel.Web.Filters
{
    public class CheckIdExistsFilter<TService, TEntity> : IActionFilter
        where TService : IBookingValidationService<TEntity, Guid>
    {
        private readonly TService _service;
        private readonly string _idParameterName;
        private readonly IBookingService _bookingService;
        public CheckIdExistsFilter(TService service, string idParameterName, IBookingService bookingService)
        {
            _service = service;
            _idParameterName = idParameterName;
            _bookingService = bookingService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue(_idParameterName, out var idValue) && idValue is Guid id)
            {
                var isExist = _service.IsExist(id).Result;
                var booking = _bookingService.Get(id);

                if (isExist && booking != null)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult($"Invalid or missing ID parameter: {_idParameterName}");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Not needed in this case
        }
    }
}