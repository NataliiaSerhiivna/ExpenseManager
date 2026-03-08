using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseManager.Common.Enums;

namespace ExpenseManager.DBModels
{
    public class WalletDBModel
    {
        //The identifier is immutable after the object is created.
        public Guid Id { get; }
        public string Name { get; set; }
        public Valuta Valuta { get; set; }
        private WalletDBModel() { }

        public WalletDBModel(string name, Valuta valuta)
        {
            Id = Guid.NewGuid();
            Name = name;
            Valuta = valuta;
        }

    }
}
