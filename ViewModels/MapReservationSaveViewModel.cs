namespace Ludo.ViewModels
{
    public class MapReservationSaveViewModel
    {
        public int ClientId { get; set; }
        public string DateFrom { get; set; }
        public string TimeFrom { get; set; }
        public string DateTo { get; set; }
        public string TimeTo { get; set; }
        public List<int> StationIds { get; set; }
        public List<int> GameIds { get; set; }
    }
}
