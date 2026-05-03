using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
                    MaxDepth = 2
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
                    MaxDepth = 2
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

        public List<Client> GetClients(string? q)
        {
            return dbContext.Clients.Where(x=> string.IsNullOrEmpty(q) || (x.Mobile.Contains(q) || (x.Firstname + " " + x.Lastname).Contains(q))).ToList();
        }

        public bool Delete(int id, int currentUserId, out bool isUsed)
        {
            isUsed = false;

            var client = GetById(id);
            if (client != null)
            {
                var clientReservation = reservationBusiness.ClientHasReservation(id);

                if(clientReservation != null)
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
                    MaxDepth = 2
                }),
                LogType = LogType.DeleteGame,
                UserId = currentUserId
            });

            return true;
        }
    }
}
