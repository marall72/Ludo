using Ludo.Models;
using System.ComponentModel.DataAnnotations;

namespace Ludo.ViewModels
{
    public class ClientListViewModel : BaseListViewModel
    {
        public ClientListViewModel()
        {
            Clients = new List<Client>();
        }
        [Display(Name = "نام، شماره تلفن، ...")]
        public string? SearchText { get; set; }
        public List<Client> Clients { get; set; }
    }
}
