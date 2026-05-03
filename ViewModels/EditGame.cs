using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class EditGame
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [MaxLength(255, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "نام")]
        public string Title { get; set; }


        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
    }
}
