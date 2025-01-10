using Microsoft.Extensions.Logging;
using MudBlazor.Services;


namespace BisleriumCafe
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddMudServices();

            IronPdf.License.LicenseKey = "IRONSUITE.TAUSHIF1TEZA.GMAIL.COM.9218-C4C9C0925C-CZRWKKOVBNHWGS-CWR7KUDVDQLI-GCXHX77TEXD5-VJKK7LKZEBJ3-UXXYNFUFTWNI-FSMG77GWWVGP-7W4CTQ-TAZT6SLDBOOLUA-DEPLOYMENT.TRIAL-47I2VM.TRIAL.EXPIRES.09.FEB.2024";


            return builder.Build();
        }
    }
}
