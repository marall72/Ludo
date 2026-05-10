using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace Ludo.Business
{
    public class StationBusiness : BaseBusiness
    {
        private ReservationBusiness reservationBusiness { get; set; }

        public StationBusiness(LudoDbContext db) : base(db)
        {
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

            model.Id = station.Id;
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(station, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
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
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(existingStation, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.EditStation,
                UserId = currentUserId
            });
            return existingStation;
        }

        public Station GetById(int id)
        {
            return dbContext.Stations.FirstOrDefault(x => x.Id == id);
        }

        public List<Station> GetStations(bool? isActive, int page, int pageSize, out int totalItemCount)
        {
            totalItemCount = dbContext.Stations.Where(x => isActive == null || x.IsActive == isActive).Count();

            return dbContext.Stations.Where(x=> isActive == null || x.IsActive == isActive).Skip((page - 1) * pageSize).Take(pageSize).ToList();
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
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.DeleteStaion,
                UserId = currentUserId
            });

            return true;
        }
    }
}
