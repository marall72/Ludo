using Ludo.Database;
using Ludo.Models;
using Ludo.ViewModels;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.Json;

namespace Ludo.Business
{
    public class UserBusiness : BaseBusiness
    {
        public UserBusiness(LudoDbContext db) : base(db)
        {
            
        }

        public EditUser Add(EditUser model, int currentUserId)
        {
            var user = new User
            {
                CreateDate = DateTime.Now,
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Mobile = model.Mobile,
                Password = model.Password,
                UpdateDate = DateTime.Now,
                Username = model.Username,
                IsAdmin = model.IsAdmin,
                IsActive = model.IsActive
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            model.Id = user.Id;

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(user, new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.AddUser,
                UserId = currentUserId
            });

            return model;
        }

        public User Update(EditUser model, int currentUserId, out bool emailTaken, out bool usernameTaken)
        {
            var existingUser = GetById(model.Id);
            emailTaken = false;
            usernameTaken = false;

            if (existingUser != null)
            {
                if (existingUser.Username != model.Username)
                {
                    var takenUser = GetByUsername(model.Username);
                    if (takenUser != null && takenUser.Id != existingUser.Id)
                    {
                        usernameTaken = true;
                        return existingUser;
                    }
                }

                if (existingUser.Email != model.Email)
                {
                    var takenUser = GetByEmail(model.Email);
                    if (takenUser != null && takenUser.Id != existingUser.Id)
                    {
                        emailTaken = true;
                        return existingUser;
                    }
                }

                existingUser.Email = model.Email;
                existingUser.Username = model.Username;
                existingUser.Mobile = model.Mobile;
                existingUser.Password = model.Password;
                existingUser.IsAdmin = model.IsAdmin;
                existingUser.Firstname = model.Firstname;
                existingUser.Lastname = model.Lastname;
                existingUser.UpdateDate = DateTime.Now;
                existingUser.IsActive = model.IsActive;

                dbContext.SaveChanges();
            }

            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(existingUser, new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.EditUser,
                UserId = currentUserId
            });

            return existingUser;
        }

        public User GetByUsername(string username)
        {
            return dbContext.Users.FirstOrDefault(x => x.Username == username);
        }

        public User GetByEmail(string email)
        {
            return dbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User GetById(int id)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public List<User> GetUsers()
        {
            return dbContext.Users.ToList();
        }

        public bool Delete(int id, int currentUserId, out bool isUsed)
        {
            var user = GetById(id);
            isUsed = false;
            if (user != null)
            {
                try
                {
                    dbContext.Remove<User>(user);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    isUsed = true;
                    return false;
                }
                
            }
            logBusiness.Add(new Log
            {
                DateTime = DateTime.Now,
                Description = JsonSerializer.Serialize(user, new JsonSerializerOptions
                {
                    MaxDepth = 2
                }),
                LogType = LogType.AddUser,
                UserId = currentUserId
            });

            return true;
        }

        public string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }

        public User ValidateUser(LoginViewModel model)
        {
            return dbContext.Users.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);
        }

        public void SaveTokenInDb(int id, string token)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                user.Token = token;
                user.TokenExpiration = DateTime.Now.AddHours(1);
                dbContext.SaveChanges();
            }
        }

        public User GetUserByToken(string token)
        {
            return dbContext.Users.FirstOrDefault(x => x.Token == token);
        }

        internal void UpdateSessionExpire(string token, DateTime newExpire)
        {
            var user = GetUserByToken(token);
            if (user != null)
            {
                user.TokenExpiration = newExpire;
                dbContext.SaveChanges();
            }
        }

        internal void DeleteSession(string token)
        {
            var user = GetUserByToken(token);
            if (user != null) {
                user.Token = "";
                user.TokenExpiration = null;
                dbContext.SaveChanges();
            }
        }
    }
}
