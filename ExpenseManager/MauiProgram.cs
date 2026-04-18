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
            builder.Services.AddSingleton<IStorageContext, FileStorageContext>();

            builder.Services.AddSingleton<IWalletRepository, WalletRepository>();
            builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();

            builder.Services.AddSingleton<IWalletService, WalletService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();

            builder.Services.AddTransient<WalletsViewModel>();
            builder.Services.AddTransient<WalletDetailsViewModel>();
            builder.Services.AddTransient<TransactionDetailsViewModel>();
            builder.Services.AddTransient<TransactionCreateViewModel>();

            builder.Services.AddTransient<WalletsPage>();
            builder.Services.AddTransient<WalletDetailsPage>();
            builder.Services.AddTransient<TransactionDetailsPage>();
            builder.Services.AddTransient<TransactionCreatePage>();
            builder.Services.AddTransient<TransactionEditViewModel>();
            builder.Services.AddTransient<TransactionEditPage>();


            builder.Services.AddTransient<WalletCreateViewModel>();
            builder.Services.AddTransient<WalletEditViewModel>();
            builder.Services.AddTransient<WalletCreatePage>();
            builder.Services.AddTransient<WalletEditPage>();

            return builder.Build();
        }
    }
}