using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseManager.Common;
using ExpenseManager.Common.Enums;
using ExpenseManager.DTOModels.Wallets;
using ExpenseManager.Services;

namespace ExpenseManager.ViewModels
{
    public partial class WalletEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IWalletService _walletService;
        private Guid _walletId;
        private EnumWithName<Valuta>[] _valutas;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private EnumWithName<Valuta> _valuta;

        [ObservableProperty]
        private Dictionary<string, string> _errors;

        public EnumWithName<Valuta>[] Valutas => _valutas;

        public WalletEditViewModel(IWalletService walletService)
        {
            _walletService = walletService;
            _valutas = EnumExtensions.GetValueWithNames<Valuta>().ToArray();
            Errors = InitErrors();
            Name = string.Empty;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _walletId = (Guid)query["WalletId"];
        }

        public async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                var wallet = await _walletService.GetWalletAsync(_walletId)
                    ?? throw new Exception("Wallet does not exist.");

                Name = wallet.Name;
                Valuta = Valutas.First(v => v.Value.Equals(wallet.Valuta));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load wallet for edit: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SaveWallet()
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
                var wallet = new WalletEditDTO(_walletId, Name, Valuta.Value);
                await _walletService.UpdateWalletAsync(wallet);

                await Shell.Current.DisplayAlertAsync("Success", "Wallet updated successfully!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to update wallet: {ex.Message}", "OK");
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