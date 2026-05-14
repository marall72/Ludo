using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Controllers
{
    [AdminAuthorization]
    public class GamesController : BaseController
    {
        private GameBusiness gameBusiness
        {
            get; set;
        }

        public GamesController(LudoDbContext db) : base(db)
        {
            gameBusiness = new GameBusiness(db);
        }

        [ViewbagAssignmentAttribute]
        public IActionResult Index(int page = 1)
        {
            var model = new GameListViewModel();

            try
            {
                if (page <= 0)
                {
                    return RedirectToAction("index", new { page = 1 });
                }

                model.Games = gameBusiness.GetGames(null, page, PagingViewModel.PageSize, out int totalItemCount);

                model.Paging = new PagingViewModel
                {
                    Action = "index",
                    Controller = "games",
                    CurrentPage = page,
                    PageCount = (int)Math.Ceiling((double)totalItemCount / PagingViewModel.PageSize),
                    TotalCount = totalItemCount
                };

                if (page > model.Paging.PageCount)
                    return RedirectToAction("index", new { page = 1 });

            }
            catch (Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            return View(model);

        }

        [HttpPost]
        public IActionResult Index(GameListViewModel model)
        {
            try
            {
                model.Games = gameBusiness.GetGames(model.SearchText, 1, PagingViewModel.PageSize, out int totalItemCount);

                model.Paging = new PagingViewModel
                {
                    Action = "index",
                    Controller = "games",
                    CurrentPage = 1,
                    PageCount = (int)Math.Ceiling((double)totalItemCount / PagingViewModel.PageSize),
                    TotalCount = totalItemCount
                };
            }
            catch(Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            
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
            var model = new EditGame();

            try
            {
                var game = gameBusiness.GetById(id);
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
            }
            catch(Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            
            return View(model);
        }

        [HttpPost]
        [Route("games/new/{id:int?}")]
        public IActionResult New(EditGame model)
        {
            try
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
                        TempData["Result"] = "بازی با موفقیت بروزرسانی شد.";
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
                        TempData["Result"] = "بازی با موفقیت ایجاد شد.";
                    }
                }

                if (ModelState.IsValid)
                    return RedirectToAction("new", "games", new { id = model.Id });

            }
            catch ( Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
            
            return View(model);
        }

        [ViewbagAssignmentAttribute]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = HttpContext.Items["User"] as User;
                var deleted = gameBusiness.Delete(id, user.Id, out bool isUsed);
                if (isUsed)
                {
                    TempData["Error"] = "به علت استفاده از این بازی در رزرو، امکان حذف آن وجود ندارد. میتوانید بازی را غیرفعال کنید.";
                }
                if (Request.Headers.Referer[0].EndsWith("/games") || deleted)
                {
                    return RedirectToAction("index");
                }
            }
            catch(Exception ex)
            {
                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = ex.Message,
                    LogType = LogType.Error,
                    UserId = CurrentUser.Id
                });
            }
           
            return RedirectToAction("new", "games", new { id = id });
        }
    }
}
