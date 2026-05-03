using Ludo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ludo.Attributes
{
    public class AdminAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as User;

            if (user == null || !user.IsAdmin)
            {
                context.Result = new RedirectToActionResult("index", "home", null);
            }
        }
    }

}
