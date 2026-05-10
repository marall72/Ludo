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
        private StationGameBusiness stationGameBusiness { get; set; }
        private GameBusiness gameBusiness { get; set; }

        public HomeController(LudoDbContext db)
        {
            userBusiness = new UserBusiness(db);
            reservationBusiness = new ReservationBusiness(db);
            clientBusiness = new ClientBusiness(db);
            stationGameBusiness = new StationGameBusiness(db);
            stationBusiness = new StationBusiness(db);
            gameBusiness = new GameBusiness(db);
        }

        [ViewbagAssignment]
        public IActionResult Index(int? id)
        {
            //todo: change alerts to modals
            //todo: a client can reserve multiple stations
            var model = new LoginViewModel();
            var clients = clientBusiness.GetClients(null, 1, 0, out int clientTotalItemCount);
            var stations = stationBusiness.GetStations(true, 1, 10000, out int stationsTotalItemCount);
            model.Reservations = new ReservationListViewModel
            {
                Reservations = reservationBusiness.GetReservations(false, 1, 10000, out int totalItemCount),
                Reservation = new EditReservation()
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

            model.Reservations.Reservation.Clients.Insert(0, new SelectListItem
            {
                Text = "انتخاب کنید"
            });

            var selectedGameIds = new List<int>();
            if (id != null && id > 0)
            {
                model.Reservations.Reservation.Id = id.Value;
                var availableReservation = reservationBusiness.GetById(id.Value);
                model.Reservations.Reservation.ToTime = availableReservation.To.ToString("HH:mm");
                model.Reservations.Reservation.ToDate = HelperMethods.ConvertMiladiToShamsi(availableReservation.To, false);
                model.Reservations.Reservation.FromTime = availableReservation.From.ToString("HH:mm");
                model.Reservations.Reservation.FromDate = HelperMethods.ConvertMiladiToShamsi(availableReservation.From, false);
                model.Reservations.Reservation.ClientId = availableReservation.ClientId;
                model.Reservations.Reservation.StationId = availableReservation.StationId;
                selectedGameIds = availableReservation.ReservationGames.Select(x => x.GameId).ToList();
            }


            var stationGames = stationGameBusiness.GetAllStationGames(model.Reservations.Reservation.Id > 0 ? model.Reservations.Reservation.StationId : stations.FirstOrDefault().Id);
            model.Reservations.Reservation.Games = gameBusiness.GetGameSelection(stationGames.Select(x => x.GameId).ToList());

            model.Reservations.Reservation.Games.SelectedGamesIds = selectedGameIds;
            model.Reservations.Reservation.Games.Games.ForEach(x =>
            {
                if (selectedGameIds.Contains(Convert.ToInt32(x.Value)))
                {
                    x.Selected = true;
                }
            });


            return View(model);
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

        public IActionResult GetStationGames(int id)
        {
            var station = stationBusiness.GetById(id);
            return PartialView("~/Views/Shared/EditorTemplates/_Games.cshtml", new GameSelection
            {
                Games = station.StationGames.Select(x => new SelectListItem { Text = x.Game.Title, Value = x.Game.Id.ToString() }).ToList()
            });
        }
    }
}
