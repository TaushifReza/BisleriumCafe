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
                Password = Utils.HashSecret(password),
                Role = role
            });

            SaveAll(users);

            Users.Add(new User
            {
                UserName = userName, 
                Password = Utils.HashSecret(password),
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

            if (user.Role == "User")
            {
                throw new Exception("Sorry you are not Eligible to Login!!!");
            }

            bool passwordIsValid = Utils.VerifyHash(password, user.Password);

            if (!passwordIsValid)
            {
                throw new Exception(loginErrorMessage);
            }

            return user;
        }

        /*internal ICollection<User> GetAllUser() => _users.ToList();*/

        public static User GetUserByUserName(string userName)
        {
            List<User> userList = GetAll();
            User user = userList.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
            {
                throw new Exception("Invalid UserName!");
            }
            return user;
        }

        public static bool CheckOrderNumber(string Username)
        {
            // Get the list of all users
            List<User> userList = GetAll();
            // Find the user with the specified username
            User user = userList.FirstOrDefault(x => x.UserName == Username);
            // Check if the order counter for the user is equal to 10
            if (user.OrderCounter == 10)
            {
                // If the order counter is 10, reset it to 0
                user.OrderCounter = 0;
                // Save the updated user list
                SaveAll(userList);
                // Return true indicating that the order number condition is met
                return true;
            }
            // If the order counter is not 10, increment it by 1
            user.OrderCounter += 1;
            // Save the updated user list
            SaveAll(userList);
            // Return false indicating that the order number condition is not met
            return false;
        }


        public static bool UserIsRegularMemeber(string Username)
        {
            // Get all order records
            List<OrderRecord> orderRecords = OrderRecordService.GetAllOrderRecord();
            // Get today's date and calculate the first day of the current month
            DateTime today = DateTime.Today;
            DateTime firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
            // Calculate the last day of the previous month
            DateTime lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            // Calculate the first day of the previous month
            DateTime firstDayOfPreviousMonth = new DateTime(lastDayOfPreviousMonth.Year, lastDayOfPreviousMonth.Month, 1);
            // Count the number of orders placed by the specified user in the previous month
            int orderInPreviousMonth = orderRecords
                .Where(order =>
                    order.CustomerIdUser.UserName == 
                    Username && order.CreateDateTime >= 
                    firstDayOfPreviousMonth &&
                    order.CreateDateTime <= 
                    lastDayOfPreviousMonth)
                .GroupBy(order => 
                    order.CreateDateTime.Day)
                .Count();
            // Check if the number of orders in the previous month is greater than or equal to 20
            if (orderInPreviousMonth >= 20)
            {
                // If the condition is true, the user is considered a regular member
                return true;
            }
            // If the condition is not met, the user is not considered a regular member
            return false;
        }
    }
}
