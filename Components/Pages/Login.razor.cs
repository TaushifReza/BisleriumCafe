using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;
using BisleriumCafe.Data.Model;
using BisleriumCafe.Data.Service;
using System.Diagnostics;

namespace BisleriumCafe.Components.Pages
{
    public partial class Login
    {
        bool _success;
        string[] _errors = { };
        MudForm? _form;


        [CascadingParameter]
        private GlobalState _globalState { get; set; }
        private bool _showDefaultCredentials { get; set; }
        private string _userName { get; set; }
        private string _password { get; set; }
        private string _errorMessage = "";

        protected override void OnInitialized()
        {
            try
            {
                var user = UserService.Login(UserService.SeedUsername, UserService.SeedPassword);
                //_showDefaultCredentials = user.HasInitialPassword;
            }
            catch { }

            _globalState.CurrentUser = null;
            _errorMessage = "";
        }

        public void LoginHandler()
        {
            Debug.WriteLine("1");
            try
            {
                Debug.WriteLine("2");
                _errorMessage = "";
                _globalState.CurrentUser = UserService.Login(_userName, _password);
                if (_globalState.CurrentUser != null)
                {
                    Debug.WriteLine("OK");
                    NavManager.NavigateTo("/");
                }
            }
            catch (Exception e)
            {
                _errorMessage = e.Message;
                Debug.WriteLine(e);
            }
        }
    }
}
