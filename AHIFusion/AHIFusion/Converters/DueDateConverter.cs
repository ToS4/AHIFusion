using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace AHIFusion;
public class DueDateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string dateString)
        {
            if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
        }
        return value;
    }
}
