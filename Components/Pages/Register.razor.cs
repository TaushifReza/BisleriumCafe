using MudBlazor;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BisleriumCafe.Data.Model;
using BisleriumCafe.Data.Service;

namespace BisleriumCafe.Components.Pages
{
    public partial class Register
    {
        private bool _success;
        private string[] _errors = { };
        MudTextField<string> _pwField1;
        MudForm _form;

        private string _userName = string.Empty;
        private string _userRole = string.Empty;
        private string _password = string.Empty;

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string _addUserErrorMessage { get; set; }

        public void AddUser()
        {
            try
            {
                Debug.WriteLine("11");
                _addUserErrorMessage = "";
                var user = new User
                {
                    UserName = _userName,
                    Password = _password,
                    Role = _userRole
                };

                UserService.RegisterNewUser(_userName, _password, _userRole);

                _form.ResetAsync();

                NavManager.NavigateTo("/");
            }
            catch (Exception e)
            {
                _addUserErrorMessage = e.Message;
            }

        }
    }
}
