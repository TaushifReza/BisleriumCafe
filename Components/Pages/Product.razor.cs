using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Components.Pages
{
    public partial class Product
    {
        private string coffee = "Coffee";

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();

        private void OpenDialog()
        {
            var options = new DialogOptions { ClassBackground = "my-custom-class" };
            DialogService.Show<DialogBlurryExample_Dialog>("Simple Dialog", options);
        }
    }
}
