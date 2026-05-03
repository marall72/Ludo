using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Controllers
{
    [AdminAuthorization]
    public class GamesController : Controller
    {
        private GameBusiness gameBusiness
        {
            get; set;
        }

        public GamesController(LudoDbContext db)
        {
            gameBusiness = new GameBusiness(db);
        }

        [ViewbagAssignmentAttribute]
        public IActionResult Index()
        {
            var model = new GameListViewModel();
            model.Games = gameBusiness.GetGames(null);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(GameListViewModel model)
        {
            model.Games = gameBusiness.GetGames(model.SearchText);
            return View(model);
        }

        public IActionResult New()
        {
            return View(new EditGame());
        }


        [Route("games/new/{id:int}")]
        [ViewbagAssignmentAttribute]
        public IActionResult New(int id)
        {
            var game = gameBusiness.GetById(id);
            var model = new EditGame();
            if (game != null)
            {
                model = new EditGame()
                {
                    Id = id,
                    Title = game.Title,
                    IsActive = game.IsActive,
                };
            }
            else if (id >= 0)
            {
                return Redirect("/games/new");
            }
            return View(model);
        }

        [HttpPost]
        [Route("games/new/{id:int?}")]
        public IActionResult New(EditGame model)
        {
            var user = HttpContext.Items["User"] as User;
            if (model.Id > 0)
            {
                var client = gameBusiness.Update(model, user.Id, out bool duplicateTitle);
                if (client == null)
                {
                    return Redirect("/games/new");
                }
                if (duplicateTitle)
                {
                    ModelState.AddModelError("", "این بازی قبلا ثبت شده است.");
                }

                if (ModelState.IsValid)
                {
                    TempData["message"] = "بازی با موفقیت بروزرسانی شد.";
                }
            }
            else
            {
                model = gameBusiness.Add(model, user.Id, out bool duplicateTitle);
                if (duplicateTitle)
                {
                    ModelState.AddModelError("", "این بازی قبلا ثبت شده است.");
                }

                if (ModelState.IsValid)
                {
                    TempData["message"] = "بازی با موفقیت ایجاد شد.";
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction("new", "games", new { id = model.Id });

            return View(model);
        }

        [ViewbagAssignmentAttribute]
        public IActionResult Delete(int id)
        {
            var user = HttpContext.Items["User"] as User;
            gameBusiness.Delete(id, user.Id, out bool isUsed);
            if (isUsed)
            {
                TempData["Error"] = "به علت استفاده از این بازی در رزرو، امکان حذف آن وجود ندارد. میتوانید بازی را غیرفعال کنید.";
            }
            if (Request.Headers.Referer[0].EndsWith("/games"))
            {
                return RedirectToAction("index");
            }

            return RedirectToAction("new", "games", new { id = id });
        }
    }
}
