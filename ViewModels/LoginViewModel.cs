using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        public ReservationListViewModel Reservations { get; set; }
        public MapEditViewModel MapEdit { get; set; }
    }
}
