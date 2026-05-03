using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            if (model.Reservation.Id > 0)
            {
                var reservation = reservationBusiness.Update(model.Reservation, user.Id, out bool crash, out bool invalidDate);
                if (reservation == null)
                {
                    //we do what?
                }
                if (crash)
                {
                    TempData["error"] = "تداخل در ثبت رزرو";
                }
                else if (invalidDate)
                {
                    TempData["error"] = "تاریخ و ساعت نامعتبر";
                }
                else
                {
                    TempData["message"] = "رزرو با موفقیت بروزرسانی شد.";
                }
            }
            else
            {
                reservationBusiness.Add(model.Reservation, user.Id, out bool crash, out bool invalidDate);
                if (crash)
                {
                    //todo: this is not working right
                    TempData["error"] = "تداخل در ثبت رزرو";
                }
                else if (invalidDate)
                {
                    TempData["error"] = "تاریخ و ساعت نامعتبر";
                }
                else
                {
                    TempData["message"] = "رزرو با موفقیت ثبت شد.";
                }
            }

            if (TempData["error"] == null || model.Reservation.Id == 0)
                return RedirectToAction("index", "home");

            return RedirectToAction("index", "home", new {id = model.Reservation.Id});
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

        [ViewbagAssignment]
        public IActionResult Index() {
            ViewBag.Result = TempData["message"] as string;
            ViewBag.Error = TempData["error"] as string;
            var model = new ReservationListViewModel
            {
                Reservations = reservationBusiness.GetReservations(true),
                IsArchive = true
            };

            return View(model);
        }
    }
}
