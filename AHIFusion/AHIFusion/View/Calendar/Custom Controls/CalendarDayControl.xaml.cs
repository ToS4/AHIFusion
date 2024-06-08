using System.Collections.ObjectModel;
using Serilog;
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
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while initializing CalendarDayControl");
            }
            finally
            {
                Log.CloseAndFlush();
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
                        ShowEventControl showEventControl = new ShowEventControl()
                        {
                            Margin = new Thickness(5),
                            Event = dayEvent
                        };

                        EventsStackPanel.Children.Add(showEventControl);

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
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
