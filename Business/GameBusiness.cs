using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using System.Text.Json;

namespace Ludo.Business
{
    public class GameBusiness : BaseBusiness
    {
        private ReservationGamesBusiness reservationGameBusiness { get; set; }
        public GameBusiness(LudoDbContext db) : base(db)
        {
            reservationGameBusiness = new ReservationGamesBusiness(db);
        }

        public EditGame Add(EditGame model, int currentUserId, out bool duplicateTitle)
        {
            duplicateTitle = false;

            var existingGame = GetByTitle(model.Title);
            if (existingGame != null)
            {
                duplicateTitle = true;
                return model;
            }

            var game = new Game
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatorId = currentUserId,
                UpdaterId = currentUserId,
                Title = model.Title,
                IsActive= model.IsActive
            };
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            model.Id = game.Id;

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(game,
                new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.AddGame,
                UserId = currentUserId
            });

            return model;
        }

        public Game Update(EditGame model, int currentUserId, out bool duplicateTitle)
        {
            var existingGame = GetById(model.Id);
            duplicateTitle = false;

            if (existingGame != null)
            {
                if (existingGame.Title != model.Title)
                {
                    var takenGame = GetByTitle(model.Title);
                    if (takenGame != null && takenGame.Id != existingGame.Id)
                    {
                        duplicateTitle = true;
                        return existingGame;
                    }
                }

                existingGame.UpdateDate = DateTime.Now;
                existingGame.UpdaterId = currentUserId;
                existingGame.Title = model.Title;
                existingGame.IsActive = model.IsActive;

                dbContext.SaveChanges();
            }

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(existingGame,
                new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.EditGame,
                UserId = currentUserId
            });

            return existingGame;
        }

        public Game GetByTitle(string title)
        {
            return dbContext.Games.FirstOrDefault(x => x.Title == title);
        }

        public Game GetById(int id)
        {
            return dbContext.Games.FirstOrDefault(x => x.Id == id);
        }

        public List<Game> GetGames(string? q)
        {
            return dbContext.Games.Where(x => string.IsNullOrEmpty(q) || (x.Title.Contains(q))).ToList();
        }

        public bool Delete(int id, int currentUserId, out bool isUsed)
        {
            isUsed = false;
            var game = GetById(id);
            if (game != null)
            {
                var reservationGame = reservationGameBusiness.GameHasReservation(id);
                if (reservationGame != null) {
                    isUsed = true;
                    return false;
                }

                dbContext.Remove<Game>(game);
                dbContext.SaveChanges();
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(game, new JsonSerializerOptions { MaxDepth = 2 }),
                LogType = LogType.DeleteGame,
                UserId = currentUserId
            });

            return true;
        }

        public GameSelection GetGameSelection(List<int> gameIds)
        {
            var model = new GameSelection(dbContext.Games.Where(x => x.IsActive && ((gameIds == null || !gameIds.Any()) || gameIds.Contains(x.Id))).OrderBy(x => x.Title).ToList());
            return model;
        }
    }
}
