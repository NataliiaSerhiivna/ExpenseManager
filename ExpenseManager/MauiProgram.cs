using Microsoft.Extensions.Logging;
using ExpenseManager.Pages;
using ExpenseManager.Storage;
using ExpenseManager.Repositories;
using ExpenseManager.Services;
using ExpenseManager.ViewModels;

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
            builder.Services.AddSingleton<IStorageContext, InMemoryStorageContext>();

            builder.Services.AddSingleton<IWalletRepository, WalletRepository>();
            builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();

            builder.Services.AddSingleton<IWalletService, WalletService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();

            builder.Services.AddTransient<WalletsViewModel>();
            builder.Services.AddTransient<WalletDetailsViewModel>();

            builder.Services.AddSingleton<WalletsPage>();
            builder.Services.AddTransient<WalletDetailsPage>();
            builder.Services.AddTransient<TransactionDetailsPage>();

            return builder.Build();
        }
    }
}