using ExpenseManager.Common.Enums;

namespace ExpenseManager.DTOModels.Wallets
{
    public class WalletCreateDTO
    {
        public string Name { get; }
        public Valuta Valuta { get; }

        public WalletCreateDTO(string name, Valuta valuta)
        {
            Name = name;
            Valuta = valuta;
        }
    }
}