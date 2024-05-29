using Microsoft.UI.Xaml.Data;

namespace AHIFusion;

public class CalendarBoolToBorderThickness : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool daySelected = (bool)value;

        if (daySelected)
        {
            return 1;
        }

        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
