using Azure;
using Azure.Core.Pipeline;
using Ludo.Attributes;
using Ludo.Business;
using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ludo.Controllers
{
    public class HomeController : Controller
    {
        private UserBusiness userBusiness { get; set; }
        private ReservationBusiness reservationBusiness { get; set; }
        private ClientBusiness clientBusiness { get; set; }
        private StationBusiness stationBusiness { get; set; }
        private GameBusiness gameBusiness { get; set; }

        public HomeController(LudoDbContext db)
        {
            userBusiness = new UserBusiness(db);
            reservationBusiness = new ReservationBusiness(db);
            clientBusiness = new ClientBusiness(db);
            stationBusiness = new StationBusiness(db);
            gameBusiness = new GameBusiness(db);
        }

        [ViewbagAssignment]
        public IActionResult Index(int? id)
        {
            //todo: change alerts to modals
            //todo: to safhe gozaresh ziad mimunim varmidare update mikone faqat 3 taie akharo miare
            //TODO: age login parid tuie hamin ajax updateha bendaze birun
            //TODO: map ham baiad update beshe ru interval
            //TODO: teste inke age mapo update nakarde bud reserve por shode bud save nashe va reacte monaseb begirim
            var model = new LoginViewModel();
            var clients = clientBusiness.GetClients(null, 1, 0, out int clientTotalItemCount);
            var stations = stationBusiness.GetStations(true, 1, 10000, out int stationsTotalItemCount);

            model.Reservations = new ReservationListViewModel
            {
                Reservations = reservationBusiness.GetReservations(null, false, 1, 10000, out int totalItemCount),
                TodaysReservationCount = reservationBusiness.GetTodaysReservationCount(),
                NewReservation = new EditReservation()
                {
                    Clients = clients.Select(x => new SelectListItem
                    {
                        Text = x.Firstname + " " + x.Lastname,
                        Value = x.Id.ToString()
                    }).ToList(),
                    Stations = stations.Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Id.ToString()
                    }).ToList()
                }
            };

            model.MapEdit = new MapEditViewModel
            {
                Stations = stationBusiness.GetStationsMap(true, DateTime.Now, DateTime.Now.AddHours(1)),
                MapReservation = model.Reservations.NewReservation
            };

            model.Reservations.NewReservation.Clients.Insert(0, new SelectListItem
            {
                Text = "انتخاب کنید"
            });

            var selectedGameIds = new List<int>();
            if (id != null && id > 0)
            {
                model.Reservations.NewReservation.Id = id.Value;
                var availableReservation = reservationBusiness.GetById(id.Value);
                model.Reservations.NewReservation.ToTime = availableReservation.To.ToString("HH:mm");
                model.Reservations.NewReservation.ToDate = HelperMethods.ConvertMiladiToShamsi(availableReservation.To, false);
                model.Reservations.NewReservation.FromTime = availableReservation.From.ToString("HH:mm");
                model.Reservations.NewReservation.FromDate = HelperMethods.ConvertMiladiToShamsi(availableReservation.From, false);
                model.Reservations.NewReservation.ClientId = availableReservation.ClientId;
                model.Reservations.NewReservation.StationIds = availableReservation.ReservationStations.Select(x=> x.StationId).ToList();
                selectedGameIds = availableReservation.ReservationGames.Select(x => x.GameId).ToList();
            }

            model.Reservations.NewReservation.Games = gameBusiness.GetGameSelection(null);

            model.Reservations.NewReservation.Games.SelectedGamesIds = selectedGameIds;
            model.Reservations.NewReservation.Games.Games.ForEach(x =>
            {
                if (selectedGameIds.Contains(Convert.ToInt32(x.Value)))
                {
                    x.Selected = true;
                }
            });


            return View(model);
        }

        public IActionResult ReservationUpdateInterval(string q)
        {
            var model = new ReservationListViewModel
            {
                Reservations = reservationBusiness.GetReservations(q, false, 1, 10000, out int totalItemCount),
                TodaysReservationCount = reservationBusiness.GetTodaysReservationCount()
            };

            return PartialView("_ReservationList", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = userBusiness.ValidateUser(model); // از DB چک کن

            if (user == null)
            {
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است.");
                return View("index");
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "شما اجازه ورود به سیستم را ندارید.");
                return View("index");
            }

            var token = userBusiness.GenerateToken();

            userBusiness.SaveTokenInDb(user.Id, token);

            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                SameSite = SameSiteMode.Strict
            });

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            var user = HttpContext.Items["User"] as User;
            if (user != null && !string.IsNullOrEmpty(user.Token))
            {
                userBusiness.DeleteSession(user.Token);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult GetUpcomingReservationsToasts()
        {
            var model = new ToastReservationCheckerViewModel();

            var reservations = reservationBusiness.GetUpcomingReservations(45);
            model.UpcomingReservations = reservations.Select(x => new ToastViewModel
            {
                Header = "مشتری تو راهه!",
                Message = $"{x.Client.Firstname} {x.Client.Lastname} ساعت {x.From.ToString("hh:mm")} رزرو داره. سیستم ها: {string.Join(",", x.ReservationStations.Select(x=> x.Station.Title))}",

            }).ToList();

            return PartialView("_ToastReservationChecker", model);
        }
    }
}
