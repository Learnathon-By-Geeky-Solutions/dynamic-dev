using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EasyTravel.Domain.Services; // Replace with your actual namespace
using EasyTravel.Application.Services; // Replace with your actual namespace
using System.Linq;
using EasyTravel.Web.Models;

namespace EasyTravel.Web.Filters
{
    public class ValidatePaymentFilter : ActionFilterAttribute
    {
        private readonly IPaymentOnlyService _paymentService;
        public ValidatePaymentFilter(IPaymentOnlyService paymentOnlyService)
        {
            _paymentService = paymentOnlyService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Guid idToCheck = Guid.Empty;

            if (context.ActionArguments.TryGetValue("bookingModel", out var modelObj) && modelObj is BookingModel model)
            {
                idToCheck = model.Id;
            }
            else if (context.ActionArguments.TryGetValue("id1", out var id1Obj) && id1Obj is Guid id1)
            {
                idToCheck = id1;
            }

            if (idToCheck != Guid.Empty)
            {
                var exists = _paymentService.IsExist(idToCheck).Result; // Adjust field name as needed
                if (exists)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }

}
