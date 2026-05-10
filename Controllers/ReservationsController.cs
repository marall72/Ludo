using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace Ludo.Controllers
{
    [LudoAuthorization]
    public class ReservationsController : Controller
    {
        private ReservationBusiness reservationBusiness { get; set; }
        public ReservationsController(LudoDbContext db)
        {
            reservationBusiness = new ReservationBusiness(db);
        }

        [HttpPost]
        [Route("reservations/new/{id:int?}")]
        public IActionResult New(ReservationListViewModel model)
        {
            var user = HttpContext.Items["User"] as User;
            var state = ModelState;
            if (model.NewReservation.Id > 0)
            {
                var reservation = reservationBusiness.Update(model.NewReservation, user.Id, out bool crash, out bool invalidDate);
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
                    TempData["message"] = "رزرو با موفقیت بروزرسانی شد.";
                }
            }
            else
            {
                reservationBusiness.Add(model.NewReservation, user.Id, out bool crash, out bool invalidDate);
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
                    TempData["message"] = "رزرو با موفقیت ثبت شد.";
                }
            }

            if (TempData["error"] == null || model.NewReservation.Id == 0)
                return RedirectToAction("index", "home");

            return RedirectToAction("index", "home", new {id = model.NewReservation.Id});
        }

        public IActionResult Delete(int id)
        {
            var user = HttpContext.Items["User"] as User;
            reservationBusiness.Delete(id, user.Id);
            TempData["Result"] = "رزرو با موفقیت حذف شد.";
            if (Request.Headers.Referer[0].EndsWith("/reservations"))
            {
                return RedirectToAction("index");
            }

            return Redirect("/home");
        }

        public IActionResult Index(int page = 1) {
            if (page <= 0)
            {
                return RedirectToAction("index", new { page = 1 });
            }

            var model = new ReservationListViewModel
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

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ReservationListViewModel model)
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

            return View(model);
        }
    }
}
