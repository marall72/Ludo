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

        public ReservationBusiness(LudoDbContext db) : base(db)
        {
            reservationGamesBusiness = new ReservationGamesBusiness(dbContext);
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

            var existingReservation = GetCrashes(from, to, model.StationId, null);
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
                StationId = model.StationId,
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

                var takenReservation = GetCrashes(from, to, model.StationId, model.Id);
                if (takenReservation != null && takenReservation.Any())
                {
                    crash = true;
                    return reservation;
                }

                reservation.UpdateDate = DateTime.Now;
                reservation.CreatorId = currentUserId;
                reservation.UpdaterId = currentUserId;
                reservation.StationId = model.StationId;
                reservation.ClientId = model.ClientId;
                reservation.Description = model.Description;
                reservation.From = from;
                reservation.To = to;

                dbContext.SaveChanges();

                reservationGamesBusiness.DeleteAllGames(reservation.Id, currentUserId);

                if (model.Games != null && model.Games.SelectedGamesIds != null && model.Games.SelectedGamesIds.Any())
                    reservationGamesBusiness.Add(model.Games.SelectedGamesIds, reservation.Id, currentUserId);
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

        public List<Reservation> GetCrashes(DateTime from, DateTime to, int stationId, int? excludedReservationId)
        {
            //this aint working right
            return dbContext.Reservations.Where(x => ((x.From >= from && x.From <= to) || (x.To >= from && x.To <= to)) && x.StationId == stationId && (excludedReservationId == null || x.Id != excludedReservationId)).ToList();
        }

        public Reservation GetById(int id)
        {
            return dbContext.Reservations.FirstOrDefault(x => x.Id == id);
        }

        public List<Reservation> GetReservations(bool isArchive)
        {
            if (isArchive)
                return dbContext.Reservations
                    .Include(x => x.ReservationGames).ThenInclude(x => x.Game)
                    .Include(x => x.Client)
                    .Include(x => x.Station)
                    .OrderBy(x => x.From).ToList();

            else
            {
                var reservations = new List<Reservation>();
                var now = DateTime.Now;
                reservations.AddRange(dbContext.Reservations
                    .Include(x => x.ReservationGames).ThenInclude(x => x.Game)
                    .Include(x => x.Client)
                    .Include(x => x.Station)
                    .OrderByDescending(x => x.To).Where(x => x.To < now).Take(3));
                reservations.AddRange(dbContext.Reservations
                    .Include(x => x.ReservationGames).ThenInclude(x => x.Game)
                    .Include(x => x.Client)
                    .Include(x => x.Station)
                    .OrderBy(x => x.To).Where(x => x.To > now && !reservations.Select(x => x.Id).Contains(x.Id)).Take(50));

                return reservations.Distinct().OrderBy(x => x.From).ToList();
            }
        }

        public List<VisitReportItem> GetReservationReport(DateTime from, DateTime to, int? clientId)
        {
            return dbContext.Reservations.Where(x => 
            ((clientId == null || clientId == 0) || x.ClientId == clientId.Value) &&
            (x.From.Date >= from.Date && x.From.Date < to.Date)).GroupBy(x=> x.ClientId).Select(x=> new VisitReportItem
            {
                ClientId = x.Key,
                ClientName = x.Where(a=> a.ClientId == x.Key).Select(a=> a.Client.Firstname + " " + a.Client.Lastname).FirstOrDefault(),
                VisitCount = x.Count()
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
