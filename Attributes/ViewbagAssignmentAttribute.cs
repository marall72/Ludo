using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

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

                if (controller.TempData["ModalResult"] != null)
                    controller.ViewBag.ModalResult = JsonSerializer.Deserialize<ModalResult>(controller.TempData["ModalResult"].ToString());
            }

            base.OnActionExecuting(context);
        }
    }
}
