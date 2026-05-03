using Ludo.Models;
using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class GameListViewModel
    {
        public GameListViewModel()
        {
            Games = new List<Game>();
        }
        public List<Game> Games { get; set; }
        [Display(Name = "عنوان بازی...")]
        public string? SearchText { get; set; }
    }
}
