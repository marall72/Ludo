using Ludo.Models;

namespace Ludo.ViewModels
{
    public class StationListViewModel
    {
        public StationListViewModel()
        {
            Stations = new List<Station>();
        }

        public List<Station> Stations { get; set; }
    }
}
