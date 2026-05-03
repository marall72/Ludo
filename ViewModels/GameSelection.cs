using Ludo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class GameSelection
    {
        public GameSelection()
        {
            Games = new List<SelectListItem>();
        }

        public GameSelection(List<Game> games)
        {
            Games = new List<SelectListItem>();
            games.ForEach(game =>
            {
                if (Games != null)
                    Games.Add(new SelectListItem { Text = game.Title, Value = game.Id.ToString()  });
            });
        }

        [Display(Name = "بازی ها")]
        public List<int> SelectedGamesIds { get; set; }

        public List<SelectListItem> Games { get; set; }
    }
}
