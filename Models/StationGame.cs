using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    [Table("StationGame")]
    public class StationGame
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int StationId { get; set; }

        [Required]
        public int GameId { get; set; }

        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }

        [ForeignKey("GameId")]
        public virtual Game Game { get; set; }
    }
}
