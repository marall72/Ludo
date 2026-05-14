using Ludo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ludo.Attributes
{
    public class LudoAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as User;
            var header = context.HttpContext.Request.Headers["X-Client-Request"];
            if (user == null)
            {
                if (!string.IsNullOrEmpty(header) && header == "fetch")
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    context.Result = new RedirectToActionResult("index", "home", null);
                }
            }
        }
    }

}
