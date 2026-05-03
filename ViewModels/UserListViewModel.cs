using Ludo.Models;

namespace Ludo.ViewModels
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; set; }
    }
}
