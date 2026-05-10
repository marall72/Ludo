namespace Ludo.ViewModels
{
    public class ToastReservationCheckerViewModel
    {
        public ToastReservationCheckerViewModel()
        {
            UpcomingReservations = new List<ToastViewModel>();
        }

        public List<ToastViewModel> UpcomingReservations { get; set; }
    }
}
