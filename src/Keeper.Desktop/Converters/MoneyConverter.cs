using Keeper.Desktop.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Keeper.Desktop.Converters
{
    class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Account a)
            {
                var accCulture = GetAccountCultureInfo(a);
                return a.Balance.ToString("C", accCulture);
            }
            else if(value is Transaction t)
            {
                var accCulture = GetAccountCultureInfo(t.Account);
                return t.Amount.ToString("C", accCulture);
            }
            return value.ToString();
        }

        private CultureInfo GetAccountCultureInfo(Account account) =>
            CultureInfo.GetCultures(CultureTypes.SpecificCultures).Where(c => new RegionInfo(c.LCID).ISOCurrencySymbol.Equals(account.Currency)).FirstOrDefault();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
                if (decimal.TryParse(s, out var amount))
                    return amount;
                else
                    return 0;
            if (value is decimal d)
                return d.ToString(CultureInfo.CurrentCulture);
            return 0;
        }
    }
}
