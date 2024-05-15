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
            // Handle the case when the key is not present in the dictionary
            // For example, you can return a default color
            return new SolidColorBrush(Colors.Gray);
        }

        return days[day] ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
