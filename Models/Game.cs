using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    [Table("Game")]
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

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

        public bool IsActive { get; set; }
    }
}
