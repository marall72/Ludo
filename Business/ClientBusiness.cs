using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ludo.Business
{
    public class ClientBusiness : BaseBusiness
    {
        private ReservationBusiness reservationBusiness { get; set; }
        public ClientBusiness(LudoDbContext db) : base(db)
        {
            reservationBusiness = new ReservationBusiness(db);
        }

        public EditClient Add(EditClient model, int currentUserId)
        {
            var client = new Client
            {
                CreateDate = DateTime.Now,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Mobile = model.Mobile,
                UpdateDate = DateTime.Now,
                CreatorId = currentUserId,
                UpdaterId = currentUserId,
                IsMale = model.Gender == Gender.Male
            };
            dbContext.Clients.Add(client);
            dbContext.SaveChanges();
            model.Id = client.Id;

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(client,
                new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.AddClient,
                UserId = currentUserId
            });
            return model;
        }

        public Client Update(EditClient model, int currentUserId, out bool mobileTaken)
        {
            var existingClient = GetById(model.Id);
            mobileTaken = false;

            if (existingClient != null)
            {
                if (existingClient.Mobile != model.Mobile)
                {
                    var takenUser = GetByMobile(model.Mobile);
                    if (takenUser != null && takenUser.Id != existingClient.Id)
                    {
                        mobileTaken = true;
                        return existingClient;
                    }
                }

                existingClient.Email = model.Email;
                existingClient.Mobile = model.Mobile;
                existingClient.Firstname = model.Firstname;
                existingClient.Lastname = model.Lastname;
                existingClient.UpdateDate = DateTime.Now;
                existingClient.UpdaterId = currentUserId;
                existingClient.IsMale = model.Gender == Gender.Male;

                dbContext.SaveChanges();
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(existingClient,
                new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.EditClient,
                UserId = currentUserId
            });
            return existingClient;
        }

        public Client GetByMobile(string mobile)
        {
            return dbContext.Clients.FirstOrDefault(x => x.Mobile == mobile);
        }

        public Client GetById(int id)
        {
            return dbContext.Clients.FirstOrDefault(x => x.Id == id);
        }

        public List<Client> GetClients(string? q, int page, int pageSize, out int totalItemCount)
        {
            totalItemCount = dbContext.Clients.Where(x => string.IsNullOrEmpty(q) || (x.Mobile.Contains(q) || (x.Firstname + " " + x.Lastname).Contains(q))).Count();

            var items = dbContext.Clients.Where(x => string.IsNullOrEmpty(q) || (x.Mobile.Contains(q) || (x.Firstname + " " + x.Lastname).Contains(q)));

            if (pageSize > 0)
            {
                return items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            return items.ToList();
        }

        public bool Delete(int id, int currentUserId, out bool isUsed)
        {
            isUsed = false;

            var client = GetById(id);
            if (client != null)
            {
                var clientReservation = reservationBusiness.ClientHasReservation(id);

                if (clientReservation != null)
                {
                    isUsed = true;
                    return false;
                }

                dbContext.Remove<Client>(client);
                dbContext.SaveChanges();
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(client,
                new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                }),
                LogType = LogType.DeleteGame,
                UserId = currentUserId
            });

            return true;
        }
    }
}
