using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ludo.Business
{
    public class StationBusiness : BaseBusiness
    {
        private ReservationBusiness reservationBusiness { get; set; }

        public StationBusiness(LudoDbContext db) : base(db)
        {
            reservationBusiness = new ReservationBusiness(db);
        }

        public EditStation Add(EditStation model, int currentUserId, out bool duplicateTitle)
        {
            duplicateTitle = false;
            var duplicateStation = GetByTitle(model.Title);
            if (duplicateStation != null)
            {
                duplicateTitle = true;
                return model;
            }

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

        public Station GetByTitle(string title)
        {
            return dbContext.Stations.FirstOrDefault(x => x.Title == title);
        }

        public Station Update(EditStation model, int currentUserId, out bool duplicateTitle)
        {
            duplicateTitle = false;

            var existingStation = GetById(model.Id);

            if (existingStation != null)
            {
                var duplicateStation = GetByTitle(model.Title);
                if (duplicateStation.Id != existingStation.Id)
                {
                    duplicateTitle = true;
                    return existingStation;
                }

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
            }
            return existingStation;
        }

        public Station GetById(int id)
        {
            return dbContext.Stations.FirstOrDefault(x => x.Id == id);
        }

        public List<Station> GetStations(bool? isActive, int page, int pageSize, out int totalItemCount)
        {
            totalItemCount = dbContext.Stations.Where(x => isActive == null || x.IsActive == isActive).Count();

            return dbContext.Stations.Where(x => isActive == null || x.IsActive == isActive).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Station> GetStationsMap()
        {
            var now = DateTime.Now;
            var result = (
                    from s in dbContext.Stations
                    from rs in s.ReservationStations
                        .Where(rs => (rs.Reservation.From <= now && rs.Reservation.To >= now) || rs == null)
                        .DefaultIfEmpty()
                    select new
                    {
                        station = s,
                        reservationStation = rs,
                        reservation = rs.Reservation
                    }
                ).ToList();

            var final = new List<Station>();
            final = result.Select(x => x.station).ToList();
           
            return final;
        }

        public bool Delete(int id, int currentUserId, out bool isUsed)
        {
            isUsed = false;
            var station = GetById(id);
            if (station != null)
            {
                var stationReservation = reservationBusiness.StationHasReservation(id);
                if (stationReservation)
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

        public void SaveMap(List<MapSaveViewModel> model, int currentUserId)
        {
            foreach (var item in model)
            {
                var station = dbContext.Stations.FirstOrDefault(x => x.Id == item.StationId);
                if (station != null) {
                    station.MapTopPosition = item.Top;
                    station.MapLeftPosition = item.Left;
                }
            }

            dbContext.SaveChanges();

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(model, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.EditStation,
                UserId = currentUserId
            });
        }
    }
}
