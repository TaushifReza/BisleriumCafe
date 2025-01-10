using System.Diagnostics;
using System.Text.Json;
using BisleriumCafe.Data.Model;

namespace BisleriumCafe.Data.Service
{
    public static class AddInService
    {
        private static void SaveAllAddIn(List<AddIn> coffees)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appAddInFilePath = Utils.GetAppAddInFilePath();

            Debug.WriteLine(appAddInFilePath);

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(coffees);
            File.WriteAllText(appAddInFilePath, json);
        }

        public static List<AddIn> GetAllAddIns()
        {
            string appAddInFilePath = Utils.GetAppAddInFilePath();
            if (!File.Exists(appAddInFilePath))
            {
                return new List<AddIn>();
            }

            var json = File.ReadAllText(appAddInFilePath);

            return JsonSerializer.Deserialize<List<AddIn>>(json);
        }

        public static void AddNewAddIn(string addInName, string addInDescription, decimal addInPrice)
        {
            List<AddIn> addIns = GetAllAddIns();
            bool addInNameExist = addIns.Any(x => x.addInName == addInName);

            if (addInNameExist)
            {
                throw new Exception("AddIn Name Exist!");
            }
            if (addInPrice <= 0 || addInPrice == null)
            {
                throw new Exception("Invalid AddIn Price!");
            }

            addIns.Add(new AddIn
            {
                addInName = addInName,
                addInDescription = addInDescription,
                addInPrice = addInPrice
            });

            SaveAllAddIn(addIns);
        }

        public static List<AddIn> GetAddIns()
        {
            return GetAllAddIns().ToList();
        }

        public static void DeleteAddIn(Guid addInId)
        {
            List<AddIn> addIns = GetAllAddIns();
            AddIn addIn = addIns.FirstOrDefault(x => x.addInId == addInId);

            if (addIn == null)
            {
                throw new Exception("Invalid AddIn ID!");
            }

            addIns.Remove(addIn);
            SaveAllAddIn(addIns);
        }

        public static void UpdateAddIn(Guid addInId, string addInName, string addInDescription, decimal addInPrice)
        {
            List<AddIn> addInList = GetAllAddIns();
            AddIn addInToUpdate = addInList.FirstOrDefault(x => x.addInId == addInId);

            if (addInToUpdate == null)
            {
                throw new Exception("AddIn Not Found!");
            }

            //coffeeToUpdate.coffeeId = coffeeId;
            addInToUpdate.addInName = addInName;
            addInToUpdate.addInDescription = addInDescription;
            addInToUpdate.addInPrice = addInPrice;
            SaveAllAddIn(addInList);
        }

        public static AddIn GetAddInByAddInName(string addInName)
        {
            List<AddIn> addInList = GetAddIns();
            AddIn addIn = addInList.FirstOrDefault(x => x.addInName == addInName);
            return addIn;
        }
    }
}
