using Ludo.Database;
using Ludo.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ludo.Business
{
    public class ReservationGamesBusiness : BaseBusiness
    {
        public ReservationGamesBusiness(LudoDbContext db) : base(db)
        {

        }

        public void Add(List<int> gameIds, int reservationId, int currentUserId)
        {
            DeleteAllGames(reservationId, currentUserId);

            if (gameIds != null && gameIds.Any())
            {
                var dbGames = gameIds.Select(x => new ReservationGame
                {
                    GameId = x,
                    ReservationId = reservationId
                });

                dbContext.ReservationGames.AddRange(dbGames);
                dbContext.SaveChanges();
            }

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(gameIds, new JsonSerializerOptions {ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true }),
                LogType = LogType.AddReservationGame,
                UserId = currentUserId
            });
        }

        public void DeleteAllGames(int reservationId, int currentUserId)
        {
            var games = GetAllReservationGames(reservationId);
            if (games != null && games.Any())
            {
                dbContext.RemoveRange(games);
                dbContext.SaveChanges();
            }

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(new { reservationId = reservationId, games = games},
                new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.DeleteReservationGame,
                UserId = currentUserId
            });
        }

        public List<ReservationGame> GetAllReservationGames(int reservationId)
        {
            return dbContext.ReservationGames.Where(x => x.ReservationId == reservationId).ToList();
        }

        public ReservationGame GameHasReservation(int gameId)
        {
            return dbContext.ReservationGames.FirstOrDefault(x => x.GameId == gameId);
        }
    }
}
