using Ludo.Database;
using Ludo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ludo.Business
{
    public class ReservationStationsBusiness : BaseBusiness
    {
        public ReservationStationsBusiness(LudoDbContext db) : base(db)
        {

        }

        public void Add(List<int> stationIds, int reservationId, int currentUserId)
        {
            DeleteAllStations(reservationId, currentUserId);

            if (stationIds != null && stationIds.Any())
            {
                var dbStations = stationIds.Select(x => new ReservationStations
                {
                    StationId = x,
                    ReservationId = reservationId
                });

                dbContext.ReservationStations.AddRange(dbStations);
                dbContext.SaveChanges();
            }

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(stationIds, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true }),
                LogType = LogType.AddedReservationStation,
                UserId = currentUserId
            });
        }

        public void DeleteAllStations(int reservationId, int currentUserId)
        {
            var stations = GetAllReservationStations(reservationId);
            if (stations != null && stations.Any())
            {
                dbContext.RemoveRange(stations);
                dbContext.SaveChanges();
            }

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(new { reservationId = reservationId, games = stations },
                new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.DeleteReservationStation,
                UserId = currentUserId
            });
        }

        public List<ReservationStations> GetAllReservationStations(int reservationId)
        {
            return dbContext.ReservationStations.Where(x => x.ReservationId == reservationId).ToList();
        }
    }
}
