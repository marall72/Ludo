using Ludo.Models;
using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class ReservationListViewModel : BaseListViewModel
    {
        public bool IsArchive { get; set; }

        [Display(Name = "نام مشتری...")]
        public string SearchText { get; set; }

        public List<Reservation> Reservations { get; set; }
        public EditReservation Reservation { get; set; }
    }
}
