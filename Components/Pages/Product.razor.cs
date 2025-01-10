using System.Diagnostics;
using BisleriumCafe.Data.Model;
using BisleriumCafe.Data.Service;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BisleriumCafe.Components.Pages
{
    public partial class Product
    {
        [CascadingParameter]
        private GlobalState _globalState { get; set; } = new();
        private string coffee = "Coffee";
        private List<Coffee> CoffeeList { get; set; }
        private List<AddIn> AddInList { get; set; }
        private string _searchString;
        private string _searchStringAddIn;

        private IEnumerable<Coffee> coffeEnumerable = new List<Coffee>();
        private IEnumerable<AddIn> addInEnumerable = new List<AddIn>();

        protected override void OnInitialized()
        {
            CoffeeList = CoffeeService.GetCoffees();
            coffeEnumerable = CoffeeService.GetCoffees();
            AddInList = AddInService.GetAddIns();
            addInEnumerable = AddInService.GetAddIns();
        }

        // quick filter - filter globally across multiple columns with the same input
        private Func<Coffee, bool> _quickFilterCoffee => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.coffeeName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if ($"{x.coffeeName}".Contains(_searchString))
                return true;

            return false;
        };

        private Func<AddIn, bool> _quickFilterAddIn => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchStringAddIn))
                return true;

            if (x.addInName.Contains(_searchStringAddIn, StringComparison.OrdinalIgnoreCase))
                return true;

            if ($"{x.addInName}".Contains(_searchStringAddIn))
                return true;

            return false;
        };

        protected override async Task OnInitializedAsync()
        {
            coffeEnumerable = CoffeeService.GetCoffees();
            addInEnumerable = AddInService.GetAddIns();
        }

        private void OpenDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            DialogService.Show<AddNewCoffeeDialog>("Add New Coffee", options);
        }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();

        public void DeleteCoffee(Guid coffeeId)
        {
            Debug.WriteLine(coffeeId);

            Snackbar.Add("Coffee Deleted", Severity.Success);

            CoffeeService.DeleteCoffee(coffeeId);
            OnInitialized();
            NavManager.NavigateTo("/product");
        }

        public void EditCoffeeOpenDialog(Coffee coffee)
        {
            var parameter = new DialogParameters<EditCoffeeDialog> { {x => x.coffee, coffee }, { x => x.ChangeState, StateHasChanged } };
            DialogService.Show<EditCoffeeDialog>("Edit Coffee", parameter);
        }

        private void OpenAddNewAddInDialog()
        {
            var parameter = new DialogParameters<AddNewAddInDialog> {
            {x => x.ChangeState, StateHasChanged  }};
            DialogService.Show<AddNewAddInDialog>("Add New AddIn", parameter);
        }

        public void DeleteAddIn(Guid addInId)
        {
            Debug.WriteLine(addInId);

            Snackbar.Add("AddIn Deleted", Severity.Success);

            AddInService.DeleteAddIn(addInId);
            //await InvokeAsync(() => StateHasChanged());
            OnInitialized();
            NavManager.NavigateTo("/product");
        }

        public void EditAddInOpenDialog(AddIn addIn)
        {
            var parameter = new DialogParameters<EditAddInDialog> { { x => x.addIn, addIn }, { x => x.ChangeState, StateHasChanged } };
            DialogService.Show<EditAddInDialog>("Edit Coffee", parameter);
        }
    }
}
