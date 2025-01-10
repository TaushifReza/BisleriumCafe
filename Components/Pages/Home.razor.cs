using BisleriumCafe.Data.Model;
using BisleriumCafe.Data.Service;

namespace BisleriumCafe.Components.Pages
{
    public partial class Home
    {
        public List<User> Users { get; set; }

        public string Title = "Home";

        private bool _hidePosition;
        private bool _loading;
        private IEnumerable<User> UserRecord = new List<User>();

        protected override void OnInitialized()
        {
            Users = UserService.GetUsers();
            UserRecord = UserService.GetUsers();
        }
    }
}
