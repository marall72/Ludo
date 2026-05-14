using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Controllers
{
    public class BaseController : Controller
    {
        internal LogBusiness logBusiness { get; set; }
        internal User CurrentUser { get { return HttpContext.Items["User"] as User; } }
        public BaseController(LudoDbContext db)
        {
            logBusiness = new LogBusiness(db);
        }
    }
}
