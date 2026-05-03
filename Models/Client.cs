using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    [Table("Client")]
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(255)]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(11)]
        public string Mobile { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public int UpdaterId { get; set; }

        [MaxLength(255)]
        public string? Email { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator{ get; set; }

        [ForeignKey("UpdaterId")]
        public User Updater { get; set; }

        public bool IsMale { get; set; }
    }
}
