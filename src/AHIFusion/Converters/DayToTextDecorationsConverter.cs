
using Microsoft.UI.Xaml.Data;
using Windows.UI.Text;

namespace AHIFusion
{
    public class DayToTextDecorationsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var day = (string)parameter;
            var days = (Dictionary<string, bool>)value;

            if (!days.ContainsKey(day))
            {
                return TextDecorations.Strikethrough;
            }
            else
            {
                if (days[day])
                {
                    return TextDecorations.Underline;
                }
                return TextDecorations.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
