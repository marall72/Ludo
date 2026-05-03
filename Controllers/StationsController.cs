using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Controllers
{
    [AdminAuthorization]
    public class StationsController : Controller
    {
        private StationBusiness stationBusiness { get; set; }
        private GameBusiness gameBusiness { get; set; }
        private StationGameBusiness stationGameBusiness { get; set; }
        public StationsController(LudoDbContext db)
        {
            stationGameBusiness = new StationGameBusiness(db);
            stationBusiness = new StationBusiness(db);
            gameBusiness = new GameBusiness(db);
        }

        [ViewbagAssignment]
        public IActionResult Index()
        {
            var model = new StationListViewModel();
            model.Stations = stationBusiness.GetStations(null);
            return View(model);
        }

        public IActionResult New()
        {
            return View(new EditStation(gameBusiness.GetGameSelection(null)) { IsActive = true, PlayerCount = 1 });
        }

        [ViewbagAssignment]
        [Route("stations/new/{id:int}")]
        public IActionResult New(int id)
        {
            var station = stationBusiness.GetById(id);
            var model = new EditStation(gameBusiness.GetGameSelection(null));
            if (station != null)
            {
                model = new EditStation(gameBusiness.GetGameSelection(null))
                {
                    Id = id,
                    Description = station.Description,
                    IsActive = station.IsActive,
                    PlayerCount = station.PlayerCount,
                    SelectedStationLevel = station.StationLevel,
                    SelectedStationType = station.StationType,
                    Title = station.Title
                };

                var stationGames = stationGameBusiness.GetAllStationGames(model.Id);
                if (stationGames != null && stationGames.Any())
                    foreach (var item in model.Games.Games.Where(x => stationGames.Select(a => a.GameId).Contains(Convert.ToInt32(x.Value))))
                    {
                        item.Selected = true;
                    }

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
                var client = stationBusiness.Update(model, user.Id);
                if (client == null)
                {
                    return Redirect("/stations/new");
                }

                if (ModelState.IsValid)
                {
                    TempData["message"] = "سیستم با موفقیت بروزرسانی شد.";
                }
            }
            else
            {

                if (ModelState.IsValid)
                {
                    model = stationBusiness.Add(model, user.Id);
                    TempData["message"] = "سیستم با موفقیت ایجاد شد.";
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

            if (Request.Headers.Referer[0].EndsWith("/stations"))
            {
                return RedirectToAction("index");
            }

            return RedirectToAction("new", "stations", new {id = id});
        }
    }
}
