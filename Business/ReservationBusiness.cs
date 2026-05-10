using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Ludo.Business
{
    public enum ReservationStatus
    {
        Past,
        Ongoing,
        Pending,
        Invalid
    }

    public class ReservationBusiness : BaseBusiness
    {
        private ReservationGamesBusiness reservationGamesBusiness { get; set; }
        private ReservationStationsBusiness reservationStationBusiness { get; set; }

        public ReservationBusiness(LudoDbContext db) : base(db)
        {
            reservationGamesBusiness = new ReservationGamesBusiness(dbContext);
            reservationStationBusiness = new ReservationStationsBusiness(dbContext);
        }

        public EditReservation Add(EditReservation model, int currentUserId, out bool crash, out bool invalidDate)
        {
            crash = invalidDate = false;
            var from = HelperMethods.ConvertShamsiToMiladi(model.FromDate, model.FromTime);
            var to = HelperMethods.ConvertShamsiToMiladi(model.ToDate, model.ToTime);

            if (from > to)
            {
                invalidDate = true;
                return model;
            }

            var existingReservation = GetCrashes(from, to, model.StationIds, null);
            if (existingReservation != null && existingReservation.Any())
            {
                crash = true;
                return model;
            }

            var reservation = new Reservation
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatorId = currentUserId,
                UpdaterId = currentUserId,
                ClientId = model.ClientId,
                Description = model.Description,
                From = from,
                To = to
            };
            dbContext.Reservations.Add(reservation);
            dbContext.SaveChanges();
            model.Id = reservation.Id;

            if (model.Games != null && model.Games.SelectedGamesIds != null && model.Games.SelectedGamesIds.Any())
            {
                reservationGamesBusiness.Add(model.Games.SelectedGamesIds, model.Id, currentUserId);
                dbContext.SaveChanges();
            }

            if (model.StationIds != null && model.StationIds.Any())
            {
                reservationStationBusiness.Add(model.StationIds, model.Id, currentUserId);
                dbContext.SaveChanges();
            }

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(model, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true }),
                LogType = LogType.AddReservation,
                UserId = currentUserId
            });
            return model;
        }

        public Reservation Update(EditReservation model, int currentUserId, out bool crash, out bool invalidDate)
        {
            var reservation = GetById(model.Id);
            crash = false;
            invalidDate = false;

            if (reservation != null)
            {
                var from = HelperMethods.ConvertShamsiToMiladi(model.FromDate, model.FromTime);
                var to = HelperMethods.ConvertShamsiToMiladi(model.ToDate, model.ToTime);

                if (from > to)
                {
                    invalidDate = true;
                    return reservation;
                }

                var takenReservation = GetCrashes(from, to, model.StationIds, model.Id);
                if (takenReservation != null && takenReservation.Any())
                {
                    crash = true;
                    return reservation;
                }

                reservation.UpdateDate = DateTime.Now;
                reservation.CreatorId = currentUserId;
                reservation.UpdaterId = currentUserId;
                reservation.ClientId = model.ClientId;
                reservation.Description = model.Description;
                reservation.From = from;
                reservation.To = to;

                dbContext.SaveChanges();

                reservationGamesBusiness.DeleteAllGames(reservation.Id, currentUserId);
                reservationStationBusiness.DeleteAllStations(reservation.Id, currentUserId);

                if (model.Games != null && model.Games.SelectedGamesIds != null && model.Games.SelectedGamesIds.Any())
                    reservationGamesBusiness.Add(model.Games.SelectedGamesIds, reservation.Id, currentUserId);

                if (model.StationIds != null && model.StationIds.Any())
                    reservationStationBusiness.Add(model.StationIds, reservation.Id, currentUserId);
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(reservation, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.EditReservation,
                UserId = currentUserId
            });
            return reservation;
        }

        public List<Reservation> GetCrashes(DateTime from, DateTime to, List<int> stationIds, int? excludedReservationId)
        {
            return dbContext.Reservations
                .Where(x => ((from >= x.From && from <= x.To) || (to >= x.From && to <= x.To))
                && x.ReservationStations.Any(a=> stationIds.Contains(a.StationId))
                && (excludedReservationId == null || x.Id != excludedReservationId))
                .ToList();
        }

        public Reservation GetById(int id)
        {
            return dbContext.Reservations.FirstOrDefault(x => x.Id == id);
        }

        public int GetTodaysReservationCount()
        {
            return dbContext.Reservations.Where(x => x.From.Date == DateTime.Now.Date).Count();
        }

        public List<Reservation> GetReservations(string q, bool isArchive, int page, int pageSize, out int totalItemCount)
        {
            totalItemCount = 0;
            if (isArchive)
            {
                totalItemCount = dbContext.Reservations.Count();

                return dbContext.Reservations
                    .Where(x => string.IsNullOrEmpty(q) || (x.Client.Firstname + " " + x.Client.Lastname).Contains(q) || x.Client.Mobile.Contains(q) || x.Client.Id.ToString() == q || x.Id.ToString() == q)
                    .Include(x => x.ReservationGames).ThenInclude(x => x.Game)
                    .Include(x => x.Client)
                    .Include(x => x.ReservationStations).ThenInclude(x=> x.Station)
                    .Skip((page - 1) * pageSize).Take(pageSize)
                    .OrderBy(x => x.From).ToList();
            }

            else
            {
                var reservations = new List<Reservation>();
                var now = DateTime.Now;
                reservations.AddRange(dbContext.Reservations
                    .Where(x => string.IsNullOrEmpty(q) || (x.Client.Firstname + " " + x.Client.Lastname).Contains(q) || x.Client.Mobile.Contains(q) || x.Client.Id.ToString() == q || x.Id.ToString() == q)
                    .Include(x => x.ReservationGames).ThenInclude(x => x.Game)
                    .Include(x => x.Client)
                    .Include(x => x.ReservationStations).ThenInclude(x => x.Station)
                    .OrderByDescending(x => x.To).Where(x => x.To < now).Take(3));

                reservations.AddRange(dbContext.Reservations
                    .Where(x => string.IsNullOrEmpty(q) || (x.Client.Firstname + " " + x.Client.Lastname).Contains(q) || x.Client.Mobile.Contains(q) || x.Client.Id.ToString() == q || x.Id.ToString() == q)
                    .Include(x => x.ReservationGames).ThenInclude(x => x.Game)
                    .Include(x => x.Client)
                    .Include(x => x.ReservationStations).ThenInclude(x => x.Station)
                    .OrderBy(x => x.To).Where(x => x.To > now && !reservations.Select(x => x.Id).Contains(x.Id)).Take(50));

                return reservations.Distinct().OrderBy(x => x.From).ToList();
            }
        }

        public List<Reservation> GetUpcomingReservations(int minutes)
        {
            var now = DateTime.Now;
            var nowLimit = now.AddMinutes(minutes);

            return dbContext.Reservations.Where(x => x.From > now && x.From < nowLimit)
                .Include(x => x.Client)
                .Include(x=> x.ReservationStations).ThenInclude(x=> x.Station)
                .OrderByDescending(x => x.From).ToList();
        }

        public List<VisitReportItem> GetReservationReport(DateTime from, DateTime to, int? clientId)
        {
            var reportRaw = dbContext.Reservations.Where(x =>
            ((clientId == null || clientId == 0) || x.ClientId == clientId.Value) &&
            (x.From.Date >= from.Date && x.From.Date < to.Date))
                .Include(x=> x.Client)
                .Include(x=> x.Creator)
                .GroupBy(x => new { x.ClientId, x.CreatorId }).Select(x => new
                {
                    ClientId = x.Key.ClientId,
                    Client = x.FirstOrDefault(a => a.ClientId == x.Key.ClientId).Client,
                    VisitCount = x.Count(),
                    Reservations = x.ToList(),
                    User = x.First().Creator
                }).ToList();

            return reportRaw.Select(x=> new VisitReportItem
            {
                ClientId = x.ClientId,
                ClientName = x.Client.Firstname + " " + x.Client.Lastname,
                Reservations = x.Reservations,
                ResponsibleUser = x.User.Firstname + " " + x.User.Lastname,
                VisitCount = x.VisitCount
            }).ToList();
        }

        public bool Delete(int id, int currentUserId)
        {
            var reservation = GetById(id);
            if (reservation != null)
            {
                dbContext.Remove<Reservation>(reservation);
                dbContext.SaveChanges();
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(reservation, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true }),
                LogType = LogType.DeleteReservation,
                UserId = currentUserId
            });

            return true;
        }

        public Reservation ClientHasReservation(int clientId)
        {
            return dbContext.Reservations.FirstOrDefault(x => x.ClientId == clientId);
        }

        public Station StationHasReservation(int stationId)
        {
            return dbContext.Stations.FirstOrDefault(x => x.Id == stationId);
        }
    }
}
