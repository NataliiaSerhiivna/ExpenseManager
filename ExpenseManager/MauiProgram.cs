using Microsoft.Extensions.Logging;
using ExpenseManager.Services;
using ExpenseManager.Pages;

namespace ExpenseManager
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
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IStorageService, StorageServices>();

            builder.Services.AddSingleton<WalletsPage>();
            builder.Services.AddTransient<WalletDetailsPage>();
            builder.Services.AddTransient<TransactionDetailsPage>();

            return builder.Build();
        }
    }
}
