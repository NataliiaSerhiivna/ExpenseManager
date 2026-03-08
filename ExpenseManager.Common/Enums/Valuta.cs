using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.Common.Enums
{
    public enum Valuta
    {
        [Display(Name = "UAH")]
        UAH,
        [Display(Name = "USD")]
        USD,
        [Display(Name = "EUR")]
        EUR,
        [Display(Name = "JPY")]
        JPY
    }
}