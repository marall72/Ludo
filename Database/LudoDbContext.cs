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
                new User { Id = 1, Firstname = "دانا", Lastname = "توانا", CreateDate = new DateTime(1993, 9,25), Email = "marall@live.com", Mobile = "09352222222", Password = "admin", UpdateDate = new DateTime(1993, 9, 25), Username = "admin", IsAdmin = true, IsActive = true },
                new User { Id = 2, Firstname = "کاربر", Lastname = "پشتیبانی", CreateDate = new DateTime(1993, 9, 25), Email = "marall@live.com", Mobile = "09351111111", Password = "support", UpdateDate = new DateTime(1993, 9, 25), Username = "support", IsAdmin = false, IsActive = true }
            );

            modelBuilder.Entity<Station>().HasData(new Station
            {
                Id = 1, CreateDate = new DateTime(2026,05,12), CreatorId = 1, IsActive = true, PlayerCount = 1, StationLevel = StationLevel.Regular, StationType = StationType.PC, Title = "R1", UpdateDate = new DateTime(2026,05,12),UpdaterId =1
            },
            new Station
            {
                Id = 2,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R2",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 3,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R3",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 4,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R4",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 5,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R5",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 6,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R6",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 7,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R7",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 8,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R8",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 9,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R9",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 10,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Regular,
                StationType = StationType.PC,
                Title = "R10",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 11,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Ultimate,
                StationType = StationType.PC,
                Title = "U1",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 12,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Ultimate,
                StationType = StationType.PC,
                Title = "U2",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 13,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Ultimate,
                StationType = StationType.PC,
                Title = "U3",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 14,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Ultimate,
                StationType = StationType.PC,
                Title = "U4",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 15,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Ultimate,
                StationType = StationType.PC,
                Title = "U5",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 16,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Ultimate,
                StationType = StationType.PC,
                Title = "U6",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 17,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Prime,
                StationType = StationType.PC,
                Title = "PR7",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 18,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Prime,
                StationType = StationType.PC,
                Title = "PR8",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 19,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Prime,
                StationType = StationType.PC,
                Title = "PR9",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 20,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Prime,
                StationType = StationType.PC,
                Title = "PR10",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 21,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Arc,
                StationType = StationType.PC,
                Title = "ARC1",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 22,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Arc,
                StationType = StationType.PC,
                Title = "ARC2",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            }, new Station
            {
                Id = 23,
                CreateDate = new DateTime(2026, 05, 12),
                CreatorId = 1,
                IsActive = true,
                PlayerCount = 1,
                StationLevel = StationLevel.Arc,
                StationType = StationType.PC,
                Title = "ARC3",
                UpdateDate = new DateTime(2026, 05, 12),
                UpdaterId = 1
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationGame> ReservationGames { get; set; }
        public DbSet<ReservationStations> ReservationStations { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
