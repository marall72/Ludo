using Ludo.Business;
using Ludo.Models;

namespace Ludo.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserBusiness userBusiness)
        {
            var token = context.Request.Cookies["auth_token"];

            if (!string.IsNullOrEmpty(token))
            {
                var user = userBusiness.GetUserByToken(token);

                if (user != null)
                {
                    if (user.TokenExpiration > DateTime.Now)
                    {
                        context.Items["User"] = user;

                        var timeLeft = user.TokenExpiration - DateTime.Now;

                        // 👇 Sliding logic
                        if (timeLeft < TimeSpan.FromMinutes(10))
                        {
                            var newExpire = DateTime.UtcNow.AddHours(1);

                            // آپدیت DB
                            userBusiness.UpdateSessionExpire(token, newExpire);

                            // آپدیت کوکی
                            context.Response.Cookies.Append("auth_token", token, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict,
                                Expires = newExpire
                            });
                        }
                    }
                    else
                    {
                        // سشن expire شده → پاک کن
                        userBusiness.DeleteSession(token);
                        context.Response.Cookies.Delete("auth_token");
                    }
                }
            }

            await _next(context);
        }
    }
}
