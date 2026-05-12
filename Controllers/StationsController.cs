using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ludo.Controllers
{
    [AdminAuthorization]
    public class StationsController : Controller
    {
        private StationBusiness stationBusiness { get; set; }
        private GameBusiness gameBusiness { get; set; }
        public StationsController(LudoDbContext db)
        {
            stationBusiness = new StationBusiness(db);
            gameBusiness = new GameBusiness(db);
        }

        [ViewbagAssignment]
        public IActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction("index", new { page = 1 });
            }

            var model = new StationListViewModel();
            model.Stations = stationBusiness.GetStations(null, page, PagingViewModel.PageSize, out int totalItemCount);

            model.Paging = new PagingViewModel
            {
                Action = "index",
                Controller = "stations",
                CurrentPage = page,
                PageCount = (int)Math.Ceiling((double)totalItemCount / PagingViewModel.PageSize),
                TotalCount = totalItemCount
            };

            if (page > model.Paging.PageCount)
                return RedirectToAction("index", new { page = 1 });

            return View(model);
        }

        public IActionResult New()
        {
            return View(new EditStation() { IsActive = true, PlayerCount = 1 });
        }

        [ViewbagAssignment]
        [Route("stations/new/{id:int}")]
        public IActionResult New(int id)
        {
            var station = stationBusiness.GetById(id);
            var model = new EditStation();
            if (station != null)
            {
                model = new EditStation()
                {
                    Id = id,
                    Description = station.Description,
                    IsActive = station.IsActive,
                    PlayerCount = station.PlayerCount,
                    SelectedStationLevel = station.StationLevel,
                    SelectedStationType = station.StationType,
                    Title = station.Title
                };

            }
            else if (id >= 0)
            {
                return Redirect("/stations/new");
            }
            return View(model);
        }

        [HttpPost]
        [Route("stations/new/{id:int?}")]
        public IActionResult New(EditStation model)
        {
            var user = HttpContext.Items["User"] as User;
            if (model.Id > 0)
            {
                var client = stationBusiness.Update(model, user.Id, out bool duplicateTitle);
                if (client == null)
                {
                    return Redirect("/stations/new");
                }

                if (duplicateTitle)
                {
                    ModelState.AddModelError("", "از این عنوان قبلا استفاده شده است.");
                    return View(model);
                }
                else
                {
                    TempData["Result"] = "سیستم با موفقیت بروزرسانی شد.";
                }
            }
            else
            {

                model = stationBusiness.Add(model, user.Id, out bool duplicateTitle);

                if (duplicateTitle)
                {
                    ModelState.AddModelError("", "از این عنوان قبلا استفاده شده است.");
                    return View(model);
                }
                else
                {
                    TempData["Result"] = "سیستم با موفقیت ایجاد شد.";
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction("new", "stations", new { id = model.Id });

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var user = HttpContext.Items["User"] as User;
            var deleted = stationBusiness.Delete(id, user.Id, out bool isUsed);
            if (isUsed)
            {
                TempData["Error"] = "به علت وجود رزرو امکان حذف این سیستم وجود ندارد.";
            }
            if (deleted)
            {
                TempData["Result"] = "سیستم با موفقیت حذف شد.";
            }

            if (Request.Headers.Referer[0].EndsWith("/stations") || deleted)
            {
                return RedirectToAction("index");
            }

            return RedirectToAction("new", "stations", new {id = id});
        }

        public IActionResult MapEdit()
        {
            var model = new MapEditViewModel();
            model.Stations = stationBusiness.GetStationsMap();
            model.IsEdit = true;
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveMap([FromBody] List<MapSaveViewModel> model)
        {
            var user = HttpContext.Items["User"] as User;
            stationBusiness.SaveMap(model, user.Id);

            return Ok();
        }
    }
}
