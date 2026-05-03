using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    [Table("User")]
    public class User
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
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(255)]
        public string? Token { get; set; }

        public DateTime? TokenExpiration { get; set; }

    }
}
