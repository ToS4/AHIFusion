using Microsoft.UI.Xaml.Data;
using Microsoft.UI;

namespace AHIFusion;

public class DayToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var day = (string)parameter;
        var days = (Dictionary<string, bool>)value;

        if (!days.ContainsKey(day))
        {
            //debug handling
            return new SolidColorBrush(Colors.Gray);
        }

        return days[day] ? new SolidColorBrush(Colors.LightGray) : new SolidColorBrush(Colors.Black);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
