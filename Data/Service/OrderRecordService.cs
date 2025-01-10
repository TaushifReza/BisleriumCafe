using System.Diagnostics;
using System.Text.Json;
using BisleriumCafe.Data.Model;

namespace BisleriumCafe.Data.Service
{
    public static class OrderRecordService
    {
        private static void SaveAllOrderRecord(List<OrderRecord> orderRecords)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appOrderRecordFilePath = Utils.GetAppOrderRecordFilePath();
            Debug.WriteLine(appOrderRecordFilePath);

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(orderRecords);
            File.WriteAllText(appOrderRecordFilePath, json);
        }

        public static List<OrderRecord> GetAllOrderRecord()
        {
            string appOrderRecordFilePath = Utils.GetAppOrderRecordFilePath();
            if (!File.Exists(appOrderRecordFilePath))
            {
                return new List<OrderRecord>();
            }

            var json = File.ReadAllText(appOrderRecordFilePath);

            return JsonSerializer.Deserialize<List<OrderRecord>>(json);
        }

        public static void AddNewOrderRecord(List<Coffee> coffeeList,
            List<AddIn> addInList, User customerIsUser)
        {
            List<OrderRecord> allOrderRecord = GetAllOrderRecord();

            //List<User> userList = UserService.GetAll();
            //User customerNameExist = userList.FirstOrDefault(x => x.UserName == customerIsUser.UserName);

            // Calculate total price for coffeeList
            decimal coffeeTotalPrice = coffeeList.Sum(Coffee => Coffee.coffeePrice);
            // Calculate total price for addInList
            decimal addInTotalPrice = addInList.Sum(AddIn => AddIn.addInPrice);
            // Calculate the overall totalPrice
            decimal NetAmount = coffeeTotalPrice + addInTotalPrice;
            decimal DiscountAmount = 0;
            decimal TotalAmount = 0;

            if (UserService.CheckOrderNumber(customerIsUser.UserName))
            {
                // UserService.CheckOrderNumber returned true, meaning the user gets the first coffee for free
                decimal firstCoffeePrice = coffeeList.FirstOrDefault()?.coffeePrice ?? 0;
                DiscountAmount += firstCoffeePrice; // Deduct the first coffee price from the DiscountAmount
            }
            if (UserService.UserIsRegularMemeber(customerIsUser.UserName))
            {
                // UserService.UserIsRegularMember returned true, meaning the user is a regular member and gets a 10% discount
                decimal regularMemberDiscountPercentage = 0.10m; // 10% discount
                DiscountAmount += (NetAmount - DiscountAmount) * regularMemberDiscountPercentage;
            }
            TotalAmount = NetAmount - DiscountAmount;

            allOrderRecord.Add(new OrderRecord
            {
                CoffeeList = coffeeList,
                AddInList = addInList,
                CustomerIdUser = customerIsUser,
                NetAmount = NetAmount,
                DiscountAmount = DiscountAmount,
                TotalAmount = TotalAmount,
            });

            SaveAllOrderRecord(allOrderRecord);

            Debug.WriteLine("OrderRecord");
        }

        public static List<OrderRecord> GetOrderRecords()
        {
            return GetAllOrderRecord().ToList();
        }
    }
}
