using Ludo.Models;
using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class ReservationListViewModel : BaseListViewModel
    {
        public bool IsArchive { get; set; }

        [Display(Name = "نام، کد، موبایل...")]
        public string SearchText { get; set; }

        public int TodaysReservationCount { get; set; }

        public List<Reservation> Reservations { get; set; }
        public EditReservation NewReservation { get; set; }
    }
}
