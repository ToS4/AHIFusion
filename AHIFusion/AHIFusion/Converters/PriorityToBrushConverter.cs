using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace AHIFusion
{
    public class PriorityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int priority = (int)value;

            switch (priority)
            {
                case 1:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
                case 2:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 165, 0));
                case 3:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 0));
                default:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


}
