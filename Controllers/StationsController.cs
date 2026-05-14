using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Net;

namespace Ludo.Controllers
{
    [LudoAuthorization]
    public class StationsController : BaseController
    {
        private StationBusiness stationBusiness { get; set; }
        private GameBusiness gameBusiness { get; set; }
        public StationsController(LudoDbContext db) : base(db)
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
            try
            {
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

        public IActionResult New()
        {
            return View(new EditStation() { IsActive = true, PlayerCount = 1 });
        }

        [ViewbagAssignment]
        [Route("stations/new/{id:int}")]
        public IActionResult New(int id)
        {
            var model = new EditStation();

            try
            {
                var station = stationBusiness.GetById(id);
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
        [Route("stations/new/{id:int?}")]
        public IActionResult New(EditStation model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var client = stationBusiness.Update(model, CurrentUser.Id, out bool duplicateTitle);
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

                    model = stationBusiness.Add(model, CurrentUser.Id, out bool duplicateTitle);

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

        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = stationBusiness.Delete(id, CurrentUser.Id, out bool isUsed);
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
            
            return RedirectToAction("new", "stations", new { id = id });
        }

        public IActionResult MapEdit()
        {
            var model = new MapEditViewModel();
            try
            {
                model.Stations = stationBusiness.GetStationsMap(null, DateTime.Now, DateTime.Now.AddHours(1));
                model.IsEdit = true;
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
        public IActionResult UpdateMap([FromBody] UpdateMapViewModel model)
        {
            var result = new MapEditViewModel();

            try
            {
                result.Stations = stationBusiness.GetStationsMap(null, HelperMethods.ConvertShamsiToMiladi(model.DateFrom, model.TimeFrom), HelperMethods.ConvertShamsiToMiladi(model.DateTo, model.TimeTo));
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
            
            return PartialView("_JustMap", result);
        }

        [HttpPost]
        public IActionResult SaveMap([FromBody] List<MapSaveViewModel> model)
        {
            try
            {
                stationBusiness.SaveMap(model, CurrentUser.Id);
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

            return Ok();
        }
    }
}
