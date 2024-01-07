using BisleriumCafe.Data.Model;
using Microsoft.VisualBasic.CompilerServices;
using System.Text.Json;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace BisleriumCafe.Data.Service
{
    public static class UserService
    {
        public static readonly List<User> Users = new();

        public const string SeedUsername = "admin";
        public const string SeedPassword = "admin";
        public const string SeedRole = "Admin";

        private static void SaveAll(List<User> users)
        {
            var appDataDirectoryPath = Utils.GetAppDirectoryPath();
            var appUsersFilePath = Utils.GetAppUsersFilePath();

            Debug.WriteLine(appUsersFilePath);

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(appUsersFilePath, json);
        }

        public static List<User> GetAll()
        {
            string appUserFilePath = Utils.GetAppUsersFilePath();
            if (!File.Exists(appUserFilePath))
            {
                return new List<User>();
            }

            var jason = File.ReadAllText(appUserFilePath);

            return JsonSerializer.Deserialize<List<User>>(jason);
        }

        public static void RegisterNewUser(string userName, string password, string role)    
        {
            List<User> users = GetAll();
            bool userNameExist = users.Any(x => x.UserName == userName);

            if (userNameExist)
            {
                throw new Exception("User Name already exist.");
            }

            users.Add(new User
            {
                UserName = userName,
                Password = password,
                Role = role
            });

            SaveAll(users);

            Users.Add(new User
            {
                UserName = userName, 
                Password = password,
                Role = role
            });
        }

        public static void SeedUsers()
        {
            var users = GetAll().FirstOrDefault(x => x.Role == "Admin");

            if (users == null)
            {
                RegisterNewUser(SeedUsername, SeedPassword, SeedRole);
            }
        }

        public static List<User> GetUsers()
        {
            return GetAll().ToList();
        }

        public static User Login(string username, string password)
        {
            const string loginErrorMessage = "Invalid username or password.";
            List<User> users = GetAll();
            User user = users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                throw new Exception(loginErrorMessage);
            }

            /*bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

            if (!passwordIsValid)
            {
                throw new Exception(loginErrorMessage);
            }*/

            return user;
        }

        /*internal ICollection<User> GetAllUser() => _users.ToList();*/
    }
}
