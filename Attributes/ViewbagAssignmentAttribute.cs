using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ludo.Attributes
{
    public class ViewbagAssignmentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                controller.ViewBag.Result = controller.TempData["Result"];
                controller.ViewBag.Error = controller.TempData["Error"];
            }

            base.OnActionExecuting(context);
        }
    }
}
