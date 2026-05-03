using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;

namespace Ludo.Business
{
    public class StationGameBusiness : BaseBusiness
    {
        public StationGameBusiness(LudoDbContext db) : base(db)
        {

        }

        public void Add(List<int> gameIds, int stationId, int currentUserId)
        {
            DeleteAllGames(stationId, currentUserId);

            if(gameIds != null && gameIds.Any())
            {
                var dbGames = gameIds.Select(x => new StationGame
                {
                    GameId = x,
                    StationId = stationId
                });

                dbContext.StationGames.AddRange(dbGames);
                dbContext.SaveChanges();

                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = JsonSerializer.Serialize(new { stationId = stationId, gameIds = gameIds}, new JsonSerializerOptions
                    {
                        MaxDepth = 2
                    }),
                    LogType = LogType.AddedStationGames,
                    UserId = currentUserId
                });
            }
        }

        public void DeleteAllGames(int stationId, int currentUserId)
        {
            var games = GetAllStationGames(stationId);
            if(games != null && games.Any())
            {
                dbContext.RemoveRange(games);
                dbContext.SaveChanges();

                logBusiness.Add(new Log
                {
                    DateTime = DateTime.Now,
                    Description = JsonSerializer.Serialize(new { stationId = stationId, games = games }, new JsonSerializerOptions
                    {
                        MaxDepth = 2
                    }),
                    LogType = LogType.DeleteStationGames,
                    UserId = currentUserId
                });
            }
        }

        public List<StationGame> GetAllStationGames(int stationId)
        {
            return dbContext.StationGames.Where(x => x.StationId == stationId).ToList();
        }
    }
}
