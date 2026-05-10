using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ludo.Controllers
{
    [AdminAuthorization]
    public class ReportsController : Controller
    {
        private ReservationBusiness reservationBusiness { get; set; }
        private ClientBusiness clientBusiness { get; set; }
        public ReportsController(LudoDbContext db)
        {
            reservationBusiness = new ReservationBusiness(db);
            clientBusiness = new ClientBusiness(db);
        }

        public IActionResult VisitReport()
        {
            var from = DateTime.Now.AddDays(-30);
            var to = DateTime.Now.AddDays(30);

            var model = new VisitReportListViewModel(from, to);

            model.Items = reservationBusiness.GetReservationReport(from, to, null);

            FillCustomers(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult VisitReport(VisitReportListViewModel model)
        {
            var fromDate = HelperMethods.ConvertShamsiToMiladi(model.FromDate, null);
            var toDate = HelperMethods.ConvertShamsiToMiladi(model.ToDate, null);

            model.Items = reservationBusiness.GetReservationReport(fromDate, toDate, model.ClientId);
            model.From = fromDate;
            model.To = toDate;
            FillCustomers(model);
            return View(model);
        }

        private void FillCustomers(VisitReportListViewModel model)
        {
            model.Clients = new List<SelectListItem>
            {
                new SelectListItem{ Text = "انتخاب کنید", Value = "0", Selected = true}
            };
            var clients = clientBusiness.GetClients(null, 1, 0, out int totalItemCount);
            if (clients != null)
            {
                model.Clients.AddRange(clients.Select(x => new SelectListItem
                {
                    Text = x.Firstname + " " + x.Lastname,
                    Value = x.Id.ToString()
                }).ToList());
            }
        }
    }
}
