using Ludo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Ludo.Database
{
    public class LudoDbContext : DbContext
    {
        public LudoDbContext(DbContextOptions<LudoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Firstname = "admin", Lastname = "admin", CreateDate = new DateTime(1993, 9,25), Email = "marall@live.com", Mobile = "09353947257", Password = "admin", UpdateDate = new DateTime(1993, 9, 25), Username = "admin", IsAdmin = true, IsActive = true },
                new User { Id = 2, Firstname = "support", Lastname = "support", CreateDate = new DateTime(1993, 9, 25), Email = "marall@live.com", Mobile = "09351111111", Password = "support", UpdateDate = new DateTime(1993, 9, 25), Username = "support", IsAdmin = false, IsActive = true }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<StationGame> StationGames { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationGame> ReservationGames { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
