using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;

namespace Ludo.Business
{
    public class StationBusiness : BaseBusiness
    {
        private StationGameBusiness stationGameBusiness { get; set; }
        private ReservationBusiness reservationBusiness { get; set; }

        public StationBusiness(LudoDbContext db) : base(db)
        {
            stationGameBusiness = new StationGameBusiness(db);
            reservationBusiness = new ReservationBusiness(db);
        }

        public EditStation Add(EditStation model, int currentUserId)
        {
            var station = new Station
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatorId = currentUserId,
                UpdaterId = currentUserId,
                StationLevel = model.SelectedStationType == StationType.PC ? model.SelectedStationLevel : null,
                StationType = model.SelectedStationType,
                Description = model.Description,
                IsActive = model.IsActive,
                PlayerCount = model.PlayerCount,
                Title = model.Title
            };
            dbContext.Stations.Add(station);
            dbContext.SaveChanges();

            var selectedGameIds = model.Games.Games.Where(x => x.Selected).Select(x => Convert.ToInt32(x.Value)).ToList();
            stationGameBusiness.Add(selectedGameIds, model.Id, currentUserId);

            model.Id = station.Id;
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(station, new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.AddStation,
                UserId = currentUserId
            });
            return model;
        }

        public Station Update(EditStation model, int currentUserId)
        {
            var existingStation = GetById(model.Id);

            if (existingStation != null)
            {
                existingStation.UpdateDate = DateTime.Now;
                existingStation.UpdaterId = currentUserId;
                existingStation.CreatorId = currentUserId;
                existingStation.StationLevel = model.SelectedStationType == StationType.PC ? model.SelectedStationLevel : null;
                existingStation.StationType = model.SelectedStationType;
                existingStation.Description = model.Description;
                existingStation.IsActive = model.IsActive;
                existingStation.PlayerCount = model.PlayerCount;
                existingStation.Title = model.Title;

                dbContext.SaveChanges();

                var selectedGameIds = model.Games.SelectedGamesIds;
                stationGameBusiness.Add(selectedGameIds, model.Id, currentUserId);
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(existingStation, new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.EditStation,
                UserId = currentUserId
            });
            return existingStation;
        }

        public Station GetById(int id)
        {
            return dbContext.Stations.Include(x => x.StationGames).ThenInclude(x => x.Game).FirstOrDefault(x => x.Id == id);
        }

        public List<Station> GetStations(bool? isActive)
        {
            return dbContext.Stations.Where(x=> isActive == null || x.IsActive == isActive).Include(x => x.StationGames).ThenInclude(x => x.Game).ToList();
        }

        public bool Delete(int id, int currentUserId, out bool isUsed)
        {
            isUsed = false;
            var station = GetById(id);
            if (station != null)
            {
                var stationReservation = reservationBusiness.StationHasReservation(id);
                if (stationReservation != null)
                {
                    isUsed = true;
                    return false;
                }

                dbContext.Remove<Station>(station);
                dbContext.SaveChanges();
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(station,
                new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.DeleteStaion,
                UserId = currentUserId
            });

            return true;
        }
    }
}
