using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManager.Common.Enums
{
    public enum Category
    {
        [Display(Name = "Products")]
        Products,
        [Display(Name = "Cafe")]
        Cafe,
        [Display(Name = "Home")]
        Home,
        [Display(Name = "Health")]
        Health
    }
}