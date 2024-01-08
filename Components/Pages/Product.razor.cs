using System.Diagnostics;
using BisleriumCafe.Data.Model;
using BisleriumCafe.Data.Service;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BisleriumCafe.Components.Pages
{
    public partial class Product
    {
        private string coffee = "Coffee";
        private List<Coffee> CoffeeList { get; set; }

        protected override void OnInitialized()
        {
            CoffeeList = CoffeeService.GetCoffees();
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

            /*CoffeeService.DeleteCoffee(coffeeId);
            NavManager.NavigateTo(NavManager.Uri, true);*/
        }

        public void EditCoffeeOpenDialog(Coffee coffee)
        {
            var parameter = new DialogParameters<EditCoffeeDialog> { {x => x.coffee, coffee }, { x => x.ChangeState, StateHasChanged } };
            DialogService.Show<EditCoffeeDialog>("Edit Coffee", parameter);
        }
    }
}
