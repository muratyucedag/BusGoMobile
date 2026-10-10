using Microsoft.Extensions.Logging;

namespace BusGoMobile
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");


                    // Inter aileleri
                    fonts.AddFont("Inter_18pt-Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter_18pt-Medium.ttf", "InterMedium");
                    fonts.AddFont("Inter_18pt-SemiBold.ttf", "InterSemiBold");
                    fonts.AddFont("Inter_18pt-Bold.ttf", "InterBold");
                    fonts.AddFont("Inter_18pt-ExtraBold.ttf", "InterExtraBold");

                    // Material Symbols ikon fontu
                    fonts.AddFont("MaterialSymbolsOutlined-Regular.ttf", "MaterialSymbols");

                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<BusGoMobile.Services.DatabaseService>();
            builder.Services.AddTransient<BusGoMobile.Pages.RegisterPage>();
            builder.Services.AddTransient<BusGoMobile.Pages.LoginPage>();
            builder.Services.AddTransient<BusGoMobile.Pages.HomePage>();
            builder.Services.AddTransient<BusGoMobile.Pages.SearchPage>();
            builder.Services.AddTransient<BusGoMobile.Pages.TripListPage>();
            builder.Services.AddTransient<BusGoMobile.Pages.SeatSelectionPage>();
            builder.Services.AddTransient<BusGoMobile.Pages.PassengerInfoPage>();
            builder.Services.AddTransient<BusGoMobile.Pages.PaymentPage>();

            return builder.Build();
        }


    }
}
