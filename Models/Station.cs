using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ludo.Models
{
    public enum StationType
    {
        PC,
        Xbox,
        PlayStation
    }

    public enum StationLevel
    {
        Regular,
        Arc,
        Ultimate,
        Prime
    }

    [Table("Station")]
    public class Station
    {
        public Station()
        {
            StationGames = new HashSet<StationGame>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "عنوان")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "تعداد کاربر")]
        public int PlayerCount{ get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        public StationType StationType { get; set; }
        public StationLevel? StationLevel { get; set; }

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

        [InverseProperty("Station")]
        public virtual ICollection<StationGame> StationGames { get; set; }
    }
}
