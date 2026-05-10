using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    public enum LogType
    {
        Error,
        AddGame,
        EditGame,
        DeleteGame,
        AddClient,
        EditClient,
        DeleteClient,
        AddReservation,
        EditReservation,
        DeleteReservation,
        AddStation,
        DeleteStaion,
        EditStation,
        AddUser,
        EditUser,
        DeleteUser,
        Login,
        Logout,
        AddReservationGame,
        DeleteReservationGame,
        AddedReservationStation,
        DeleteReservationStation
    }

    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public LogType LogType { get; set; }

        public string Description { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
