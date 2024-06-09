using System.Collections.ObjectModel;
using AHIFusion.Model;
using Serilog;
using Windows.UI;
using Windows.UI.Core;

namespace AHIFusion
{
	public sealed partial class CalendarDayControl : UserControl
	{
        public static readonly DependencyProperty DaySelectedProperty = DependencyProperty.Register("DaySelected", typeof(bool), typeof(CalendarDayControl), new PropertyMetadata(false));
        public bool DaySelected
        {
            get { return (bool)GetValue(DaySelectedProperty); }
            set { SetValue(DaySelectedProperty, value); }
        }

        public CalendarDay Day;

        public CalendarDayControl()
		{
            try
            {
                this.InitializeComponent();
                var lighterColor = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[ThemeConfig.theme])["BackgroundColor"];
                MainBorder.BorderBrush = new SolidColorBrush(lighterColor);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while initializing CalendarDayControl");
            }
        }

        public void UpdateDay()
        {
            try
            {
                Log.Information("Updating day");

                DateTextBlock.Text = Day.Date.ToString();

                EventsStackPanel.Children.Clear();

                foreach (DayEvent dayEvent in EventCollection.Events)
                {
                    if (dayEvent.Date == Day.Date)
                    {
                        var color = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[ThemeConfig.theme])["TertiaryColor"];
                        var brushColor = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[ThemeConfig.theme])["OutlineColor"];

                        Border border = new Border()
                        {
                            BorderBrush = new SolidColorBrush(brushColor),
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(5),
                            Margin = new Thickness(5),
                            Background = new SolidColorBrush(color)
                        };

                        ShowEventControl showEventControl = new ShowEventControl()
                        {
                            Event = dayEvent
                        };

                        border.Child = showEventControl;
                        EventsStackPanel.Children.Add(border);

                        showEventControl.UpdateEvent();
                    }
                }

                int amountEvents = EventsStackPanel.Children.Count;

                if (amountEvents > 3)
                {
                    for (int i = 0; i < (amountEvents - 3); i++)
                    {
                        EventsStackPanel.Children.RemoveAt(EventsStackPanel.Children.Count - 1);
                    }

                    TextBlock textBlock = new TextBlock()
                    {
                        Text = $"+{amountEvents - 3}",
                        TextAlignment = TextAlignment.Center,
                    };

                    EventsStackPanel.Children.Add(textBlock);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating day");
            }
        }

        private void Border_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var darkerColor = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[ThemeConfig.theme])["BackgroundColorDarker"];
            MainBorder.BorderBrush = new SolidColorBrush(darkerColor);
        }

        private void Border_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var lighterColor = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[ThemeConfig.theme])["BackgroundColor"];
            MainBorder.BorderBrush = new SolidColorBrush(lighterColor);
        }
    }
}
