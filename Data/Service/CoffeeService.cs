using System.Diagnostics;
using System.Text.Json;
using BisleriumCafe.Data.Model;

namespace BisleriumCafe.Data.Service
{
    public static class CoffeeService
    {
        private static void SaveAll(List<Coffee> coffees)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appCoffeeFilePath = Utils.GetAppCoffeeFilePath();

            Debug.WriteLine(appCoffeeFilePath);

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(coffees);
            File.WriteAllText(appCoffeeFilePath, json);
        }

        public static List<Coffee> GetAllCoffees()
        {
            string appCoffeeFilePath = Utils.GetAppCoffeeFilePath();
            if (!File.Exists(appCoffeeFilePath))
            {
                return new List<Coffee>();
            }

            var json = File.ReadAllText(appCoffeeFilePath);

            return JsonSerializer.Deserialize<List<Coffee>>(json);
        }

        public static void AddNewCoffee(string coffeeName, string coffeeDescription, decimal coffeePrice)
        {
            List<Coffee> coffees = GetAllCoffees();
            bool coffeeNameExist = coffees.Any(x => x.coffeeName == coffeeName);

            if (coffeeNameExist)
            {
                throw new Exception("Coffee Name Already Exist!");
            }
            if (coffeePrice == 0 || coffeePrice <= 0 || coffeePrice == null)
            {
                throw new Exception("Invalid Coffee Price!");
            }

            coffees.Add(new Coffee
            {
                coffeeName = coffeeName,
                coffeeDescription = coffeeDescription,
                coffeePrice = coffeePrice
            });

            SaveAll(coffees);
        }

        public static List<Coffee> GetCoffees()
        {
            return GetAllCoffees().ToList();
        }

        public static void DeleteCoffee(Guid coffeeId)
        {
            List<Coffee> coffees = GetAllCoffees();
            Coffee coffee = coffees.FirstOrDefault(x =>  x.coffeeId == coffeeId);

            if (coffee == null)
            {
                throw new Exception("Invalid Coffee ID.");
            }

            coffees.Remove(coffee);
            SaveAll(coffees);
        }

        public static void UpdateCoffee(Guid coffeeId, string coffeeName, string coffeeDescription, decimal coffeePrice)
        {
            List<Coffee> coffeeList = GetAllCoffees();
            Coffee coffeeToUpdate = coffeeList.FirstOrDefault(x => x.coffeeId == coffeeId);

            if (coffeeToUpdate == null)
            {
                throw new Exception("Coffee Not Found!");
            }

            //coffeeToUpdate.coffeeId = coffeeId;
            coffeeToUpdate.coffeeName = coffeeName;
            coffeeToUpdate.coffeeDescription = coffeeDescription;
            coffeeToUpdate.coffeePrice = coffeePrice;
            SaveAll(coffeeList);
        }

        public static Coffee GetCoffeeByCoffeeName(string coffeeName)
        {
            List<Coffee> coffeeList = GetCoffees();
            Coffee coffee = coffeeList.FirstOrDefault(x => x.coffeeName == coffeeName);

            return coffee;
        }
    }
}
