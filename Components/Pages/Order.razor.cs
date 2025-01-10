using System.Diagnostics;
using System.Globalization;
using BisleriumCafe.Data.Model;
using BisleriumCafe.Data.Service;
using MudBlazor;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

namespace BisleriumCafe.Components.Pages
{
    public partial class Order
    {
        public string title = "Order";
        private List<OrderRecord> _orderRecords { get; set; }
        private string selectedMonth = DateTime.Now.ToString("MMMM yyyy");

        private IEnumerable<OrderRecord> OrderRecord = new List<OrderRecord>();


        protected override void OnInitialized()
        {
            _orderRecords = OrderRecordService.GetOrderRecords();
            OrderRecord = OrderRecordService.GetOrderRecords();
        }

        public List<string> GetUniqueMonths()
        {
            // Extract unique months from _orderRecords
            var uniqueMonths = _orderRecords
                .Select(order => order.CreateDateTime.ToString("MMMM yyyy"))
                .Distinct()
                .ToList();

            return uniqueMonths;
        }

        private void GeneratePdfReport()
        {
            if (string.IsNullOrEmpty(selectedMonth))
            {
                Snackbar.Add("No month selected.", Severity.Warning);
            }
            else
            {
                // Convert the selectedMonth string to DateTime for the report generation function
                DateTime selectedDateTime = DateTime.ParseExact(selectedMonth, "MMMM yyyy", CultureInfo.InvariantCulture);

                // Call the function to generate the PDF report for the selected month
                GeneratePdfTop5ReportForSelectedMonth(selectedDateTime);
            }
        }

        private void OpenAddToCartDialog()
        {
            var options = new DialogParameters<AddNewOrderDialog>() { { x => x._orderRecords, _orderRecords  } };
            DialogService.Show<AddNewOrderDialog>("Add New OrderRecord", options);
        }

        public void GeneratePdfTop5ReportForToday()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now;

            // Filter order records for the current date
            var filteredOrders = _orderRecords
                .Where(order => order.CreateDateTime.Date == currentDate.Date);
            if (filteredOrders.Count() == 0)
            {
                Snackbar.Add("No orders found for today.", Severity.Warning);
            }
            else
            {
                // Flatten the list of coffee and add-ins from filtered order records
                var allCoffees = filteredOrders.SelectMany(order => order.CoffeeList);
                var allAddIns = filteredOrders.SelectMany(order => order.AddInList);

                // Group and count occurrences of each coffee type
                var topCoffeeTypes = allCoffees
                    .GroupBy(coffee => coffee.coffeeName)
                    .OrderByDescending(group => group.Count())
                    .Take(5)
                    .Select(group => new
                    {
                        CoffeeType = group.Key,
                        Quantity = group.Count(),
                        Revenue = group.Sum(coffee => coffee.coffeePrice)
                    });

                // Group and count occurrences of each add-in
                var topAddIns = allAddIns
                    .GroupBy(addIn => addIn.addInName)
                    .OrderByDescending(group => group.Count())
                    .Take(5)
                    .Select(group => new
                    {
                        AddInName = group.Key,
                        Quantity = group.Count(),
                        Revenue = group.Sum(addIn => addIn.addInPrice)
                    });

                // File-path setup
                var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"invoice_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var fullPath = Path.Combine(directoryPath, fileName);

                // PDF Setup and Generation
                QuestPDF.Settings.License = LicenseType.Community;

                Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(2, Unit.Centimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x.FontSize(20));

                            page.Header()
                                .Text("Purchases/Sales Transactions!!!")
                                .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                            page.Content()
                                .PaddingVertical(1, Unit.Centimetre)
                                .Column(x =>
                                {
                                    x.Spacing(20);

                                    // Print the top 5 most frequently purchased coffee types for today
                                    x.Item().Text("Top 5 Coffee Types for Today:"); 
                                    foreach (var coffeeType in topCoffeeTypes)
                                    {
                                        x.Item().Text($"Coffee Type: {coffeeType.CoffeeType}, Quantity:" +
                                                      $"{coffeeType.Quantity}, Revenue: {coffeeType.Revenue:C}");
                                    }
                                    x.Item().Text("");
                                    // Print the top 5 most frequently purchased add-ins for today
                                    x.Item().Text("Top 5 Add-Ins for Today:"); 
                                    foreach (var addIn in topAddIns)
                                    {
                                        x.Item().Text($"Add-In: {addIn.AddInName}, Quantity:" +
                                                      $"{addIn.Quantity}, Revenue: {addIn.Revenue:C}");
                                    }
                                });

                            page.Footer()
                                .AlignCenter()
                                .Text(x =>
                                {
                                    x.Span("Page ");
                                    x.CurrentPageNumber();
                                });
                        });
                    })
                    .GeneratePdf(fullPath);
                Snackbar.Add("PDF of Today is Generated", Severity.Success);
            }
        }

        public void GeneratePdfTop5ReportForSelectedMonth(DateTime selectedMonth)
        {
            // Get the first day of the current month
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Filter order records for the selected month
            var filteredOrders = _orderRecords
                .Where(order => order.CreateDateTime.Year == selectedMonth.Year &&
                                order.CreateDateTime.Month == selectedMonth.Month);


            // Flatten the list of coffee and add-ins from filtered order records
            var allCoffees = filteredOrders.SelectMany(order => order.CoffeeList);
            var allAddIns = filteredOrders.SelectMany(order => order.AddInList);

            // Group and count occurrences of each coffee type
            var topCoffeeTypes = allCoffees
                .GroupBy(coffee => coffee.coffeeName)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => new
                {
                    CoffeeType = group.Key,
                    Quantity = group.Count(),
                    Revenue = group.Sum(coffee => coffee.coffeePrice)
                });

            // Group and count occurrences of each add-in
            var topAddIns = allAddIns
                .GroupBy(addIn => addIn.addInName)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => new
                {
                    AddInName = group.Key,
                    Quantity = group.Count(),
                    Revenue = group.Sum(addIn => addIn.addInPrice)
                });

            // Print the top 5 most frequently purchased coffee types for the current month
            Debug.WriteLine("Top 5 Coffee Types for the Current Month:");
            foreach (var coffeeType in topCoffeeTypes)
            {
                Debug.WriteLine($"Coffee Type: {coffeeType.CoffeeType}, Quantity: {coffeeType.Quantity}, Revenue: {coffeeType.Revenue:C}");
            }

            // Print the top 5 most frequently purchased add-ins for the current month
            Debug.WriteLine("\nTop 5 Add-Ins for the Current Month:");
            foreach (var addIn in topAddIns)
            {
                Debug.WriteLine($"Add-In: {addIn.AddInName}, Quantity: {addIn.Quantity}, Revenue: {addIn.Revenue:C}");
            }

            // File-path setup
            var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"invoice_{selectedMonth:MMMM yyyy}{DateTime.Now:MMddHHmmss}.pdf";
            var fullPath = Path.Combine(directoryPath, fileName);

            // PDF Setup and Generation
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));

                        page.Header()
                            .Text("Purchases/Sales Transactions!!!")
                            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(x =>
                            {
                                x.Spacing(20);

                                // Print the top 5 most frequently purchased coffee types for the current month
                                x.Item().Text("Top 5 Coffee Types for the Current Month:"); 
                                foreach (var coffeeType in topCoffeeTypes)
                                {
                                    x.Item().Text($"Coffee Type: {coffeeType.CoffeeType}, Quantity:" +
                                                  $"{coffeeType.Quantity}, Revenue:" +
                                                  $"{coffeeType.Revenue:C}");
                                }

                                // Print the top 5 most frequently purchased add-ins for the current month
                                x.Item().Text("");
                                x.Item().Text("Top 5 Add-Ins for the Current Month:"); 
                                foreach (var addIn in topAddIns)
                                {
                                    x.Item().Text($"Add-In: {addIn.AddInName}, Quantity: {addIn.Quantity}," +
                                                  $"Revenue: {addIn.Revenue:C}");
                                }
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                    });
                })
                .GeneratePdf(fullPath);
            Snackbar.Add($"PDF for {selectedMonth:MMMM yyyy} is Generated", Severity.Success);
        }


    }
}
