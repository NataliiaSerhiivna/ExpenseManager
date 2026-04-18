using ExpenseManager.Common.Enums;

namespace ExpenseManager.DTOModels.Wallets
{
    public class WalletEditDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public Valuta Valuta { get; }

        public WalletEditDTO(Guid id, string name, Valuta valuta)
        {
            Id = id;
            Name = name;
            Valuta = valuta;
        }
    }
}