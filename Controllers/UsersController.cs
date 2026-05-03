using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Ludo.Controllers
{
    [AdminAuthorization]
    public class UsersController : Controller
    {
        private UserBusiness userBusiness { get; set; }

        public UsersController(LudoDbContext db)
        {
            userBusiness = new UserBusiness(db);
        }

        [ViewbagAssignment]
        public IActionResult Index()
        {
            var model = new UserListViewModel();
            model.Users = userBusiness.GetUsers();
            return View(model);
        }

        public IActionResult New()
        {
            return View(new EditUser());
        }

        [ViewbagAssignment]
        [Route("users/new/{id:int}")]
        public IActionResult New(int id)
        {
            ViewBag.Result = TempData["message"] as string;
            var user = userBusiness.GetById(id);
            var model = new EditUser();
            if (user != null)
            {
                model = new EditUser()
                {
                    Id = id,
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    IsAdmin = user.IsAdmin,
                    Mobile = user.Mobile,
                    Password = user.Password,
                    Username = user.Username,
                    IsActive = user.IsActive
                };
            }
            else if(id >= 0)
            {
                return Redirect("/users/new");
            }
            return View(model);
        }

        [HttpPost]
        [Route("users/new/{id:int?}")]
        public IActionResult New(EditUser model)
        {
            var currentUser = HttpContext.Items["User"] as User;
            if (model.Id > 0)
            {
                var user = userBusiness.Update(model, currentUser.Id, out bool emailTaken, out bool usernameTaken);
                if(user == null)
                {
                    return Redirect("/users/new");
                }

                if (emailTaken) {
                    ModelState.AddModelError("", "این ایمیل قبلا استفاده شده است.");
                }
                if (usernameTaken)
                {
                    ModelState.AddModelError("", "این نام کاربری قبلا استفاده شده است.");
                }

                if (ModelState.IsValid)
                {
                    TempData["message"] = "کاربر با موفقیت بروزرسانی شد.";
                }
            }
            else
            {
                var existingUser = userBusiness.GetByUsername(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "این نام کاربری قبلا استفاده شده است.");
                }

                existingUser = userBusiness.GetByEmail(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "این ایمیل قبلا استفاده شده است.");
                }

                if (ModelState.IsValid)
                {
                    userBusiness.Add(model, currentUser.Id);
                    TempData["message"] = "کاربر با موفقیت ایجاد شد.";
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("new", "users", new { id = model.Id });
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var currentUser = HttpContext.Items["User"] as User;
            var deleted = userBusiness.Delete(id, currentUser.Id, out bool isUsed);

            if (isUsed)
            {
                TempData["Error"] = "این کاربر در سیستم فعالیت انجام داده و امکان حذف وی وجود ندارد.";
            }
            else
            {
                TempData["Result"] = "کاربر با موفقیت حذف شد.";
            }

            if (Request.Headers.Referer[0].EndsWith("/users"))
            {
                return RedirectToAction("index");
            }

            return RedirectToAction("new", "users", new { id = id });
        }
    }
}
