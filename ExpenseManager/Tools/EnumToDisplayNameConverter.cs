using ExpenseManager.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ExpenseManager.Tools
{
    public class EnumToDisplayNameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            if (value is not Enum castedEnum)
                return value.ToString() ?? string.Empty;

            return castedEnum.GetDisplayName();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}