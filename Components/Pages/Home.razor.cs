using BisleriumCafe.Data.Model;
using BisleriumCafe.Data.Service;

namespace BisleriumCafe.Components.Pages
{
    public partial class Home
    {
        public List<User> Users { get; set; }

        public string Title = "Home";

        protected override void OnInitialized()
        {
            Users = UserService.GetUsers();
        }
    }
}
