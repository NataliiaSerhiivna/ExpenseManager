using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseManager.Common;
using ExpenseManager.Common.Enums;
using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Services;

namespace ExpenseManager.ViewModels
{
    public partial class WalletCreateViewModel : BaseViewModel
    {
        private readonly IWalletService _walletService;
        private EnumWithName<Valuta>[] _valutas;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private EnumWithName<Valuta> _valuta;

        [ObservableProperty]
        private Dictionary<string, string> _errors;

        public EnumWithName<Valuta>[] Valutas => _valutas;

        public WalletCreateViewModel(IWalletService walletService)
        {
            _walletService = walletService;
            _valutas = EnumExtensions.GetValueWithNames<Valuta>().ToArray();
            Errors = InitErrors();
            Name = string.Empty;
        }

        [RelayCommand]
        public async Task CreateWallet()
        {
            IsBusy = true;

            var errors = Validators.ValidateWallet(Name, Valuta?.Value);
            Errors = InitErrors();

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    if (string.IsNullOrWhiteSpace(Errors[error.MemberName]))
                    {
                        Errors[error.MemberName] = error.ErrorMessage;
                        continue;
                    }

                    Errors[error.MemberName] += Environment.NewLine + error.ErrorMessage;
                }

                OnPropertyChanged(nameof(Errors));
                IsBusy = false;
                return;
            }

            try
            {
                var newWallet = new WalletCreateDTO(Name, Valuta.Value);
                await _walletService.CreateWalletAsync(newWallet);

                await Shell.Current.DisplayAlertAsync("Success", "Wallet created successfully!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to create wallet: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Back()
        {
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to navigate back: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Dictionary<string, string> InitErrors()
        {
            return new Dictionary<string, string>()
            {
                { nameof(Name), string.Empty },
                { nameof(Valuta), string.Empty }
            };
        }
    }
}