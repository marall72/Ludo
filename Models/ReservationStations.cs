using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    [Table("ReservationStations")]
    public class ReservationStations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public int StationId { get; set; }

        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; }

        [ForeignKey("StationId")]
        public Station Station { get; set; }

    }
}
