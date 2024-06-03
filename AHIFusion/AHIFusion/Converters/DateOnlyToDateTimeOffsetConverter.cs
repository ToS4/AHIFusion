using System;
using Microsoft.UI.Xaml.Data;

namespace AHIFusion;
public class DateOnlyToDateTimeOffsetConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateOnly dateOnly)
        {
            return new DateTimeOffset(dateOnly.ToDateTime(TimeOnly.MinValue));
        }
        return DateTimeOffset.MinValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
