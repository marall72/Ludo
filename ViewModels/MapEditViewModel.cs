using Ludo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ludo.ViewModels
{
    public class MapEditViewModel
    {
        public DateTime LastUpdate { get { return DateTime.Now; } }
        public List<Station> Stations { get; set; }
        public bool IsEdit { get; set; }
        public EditReservation MapReservation { get; set; }
    }

}
