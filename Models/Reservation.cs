using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    [Table("Reservation")]
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }

        [MaxLength(1000)]
        public string? Description{ get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public int UpdaterId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }

        [ForeignKey("UpdaterId")]
        public User Updater { get; set; }

        [InverseProperty("Reservation")]
        public virtual ICollection<ReservationGame> ReservationGames { get; set; }


        [InverseProperty("Reservation")]
        public virtual ICollection<ReservationStations> ReservationStations { get; set; }

    }
}
