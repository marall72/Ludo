using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO.Compression;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace Ludo.Controllers
{
    [LudoAuthorization]
    public class ReservationsController : BaseController
    {
        private ReservationBusiness reservationBusiness { get; set; }
        public ReservationsController(LudoDbContext db) : base(db)
        {
            reservationBusiness = new ReservationBusiness(db);
        }

        [HttpPost]
        [Route("reservations/new/{id:int?}")]
        public IActionResult New(ReservationListViewModel model)
        {
            try
            {
                var state = ModelState;
                if (model.NewReservation.Id > 0)
                {
                    var reservation = reservationBusiness.Update(model.NewReservation, CurrentUser.Id, out bool crash, out bool invalidDate);
                    if (reservation == null)
                    {
                        //we do what?
                    }
                    if (crash)
                    {
                        TempData["ModalResult"] = JsonSerializer.Serialize(new ModalResult
                        {
                            IsError = true,
                            Message = "تداخل در ثبت رزرو"
                        });
                    }
                    else if (invalidDate)
                    {
                        TempData["ModalResult"] = JsonSerializer.Serialize(new ModalResult
                        {
                            IsError = true,
                            Message = "تاریخ و ساعت نامعتبر"
                        });
                    }
                    else
                    {
                        TempData["Result"] = "رزرو با موفقیت بروزرسانی شد.";
                    }
                }
                else
                {
                    reservationBusiness.Add(model.NewReservation, CurrentUser.Id, out bool crash, out bool invalidDate);
                    if (crash)
                    {
                        TempData["ModalResult"] = JsonSerializer.Serialize(new ModalResult
                        {
                            IsError = true,
                            Message = "تداخل در ثبت رزرو"
                        });
                    }
                    else if (invalidDate)
                    {
                        TempData["ModalResult"] = JsonSerializer.Serialize(new ModalResult
                        {
                            IsError = true,
                            Message = "تاریخ و ساعت نامعتبر"
                        });
                    }
                    else
                    {
                        TempData["Result"] = "رزرو با موفقیت ثبت شد.";
                    }
                }

                if (TempData["error"] == null || model.NewReservation.Id == 0)
                    return RedirectToAction("index", "home");
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

            return RedirectToAction("index", "home", new { id = model.NewReservation.Id });
        }

        public IActionResult Delete(int id)
        {
            try
            {
                reservationBusiness.Delete(id, CurrentUser.Id);
                TempData["Result"] = "رزرو با موفقیت حذف شد.";
                if (Request.Headers.Referer[0].EndsWith("/reservations"))
                {
                    return RedirectToAction("index");
                }
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
           
            return Redirect("/home");
        }

        public IActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction("index", new { page = 1 });
            }
            var model = new ReservationListViewModel();
            try
            {
                model = new ReservationListViewModel
                {
                    Reservations = reservationBusiness.GetReservations(null, true, page, PagingViewModel.PageSize, out int totalItemCount),
                    IsArchive = true,
                    TodaysReservationCount = reservationBusiness.GetTodaysReservationCount()
                };

                model.Paging = new PagingViewModel
                {
                    Action = "index",
                    Controller = "reservations",
                    CurrentPage = page,
                    PageCount = (int)Math.Ceiling((double)totalItemCount / PagingViewModel.PageSize),
                    TotalCount = totalItemCount
                };

                if (page > model.Paging.PageCount)
                    return RedirectToAction("index", new { page = 1 });
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
        public IActionResult Index(ReservationListViewModel model)
        {
            try
            {
                model.Reservations = reservationBusiness.GetReservations(model.SearchText, true, 1, PagingViewModel.PageSize, out int totalItemCount);
                model.IsArchive = true;
                model.TodaysReservationCount = reservationBusiness.GetTodaysReservationCount();

                model.Paging = new PagingViewModel
                {
                    Action = "index",
                    Controller = "reservations",
                    CurrentPage = 1,
                    PageCount = (int)Math.Ceiling((double)totalItemCount / PagingViewModel.PageSize),
                    TotalCount = totalItemCount
                };
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
        public IActionResult SaveMapReservation([FromBody] MapReservationSaveViewModel model)
        {
            try
            {
                reservationBusiness.Add(new EditReservation
                {
                    ClientId = model.ClientId,
                    FromDate = model.DateFrom,
                    FromTime = model.TimeFrom,
                    ToDate = model.DateTo,
                    Games = new GameSelection
                    {
                        SelectedGamesIds = model.GameIds
                    },
                    StationIds = model.StationIds,
                    ToTime = model.TimeTo
                }, CurrentUser.Id, out bool crash, out bool invalidDate);
                if (crash)
                {
                    return StatusCode((int)HttpStatusCode.Conflict);
                }
                else if (invalidDate)
                {
                    return StatusCode(500);
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
            
            return Ok();
        }

        public IActionResult ReservationUpdateInterval(string q)
        {
            var model = new ReservationListViewModel();
            try
            {
                model = new ReservationListViewModel
                {
                    Reservations = reservationBusiness.GetReservations(q, false, 1, 10000, out int totalItemCount),
                    TodaysReservationCount = reservationBusiness.GetTodaysReservationCount()
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
           
            return PartialView("_ReservationList", model);
        }

        public IActionResult GetUpcomingReservationsToasts()
        {
            var model = new ToastReservationCheckerViewModel();
            try
            {
                var reservations = reservationBusiness.GetUpcomingReservations(45);
                model.UpcomingReservations = reservations.Select(x => new ToastViewModel
                {
                    Header = "مشتری تو راهه!",
                    Message = $"{x.Client.Firstname} {x.Client.Lastname} ساعت {x.From.ToString("hh:mm")} رزرو داره. سیستم ها: {string.Join(",", x.ReservationStations.Select(x => x.Station.Title))}",

                }).ToList();
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
            
            return PartialView("_ToastReservationChecker", model);
        }
    }
}
