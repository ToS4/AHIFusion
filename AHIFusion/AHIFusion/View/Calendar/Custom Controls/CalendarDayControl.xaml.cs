using System.Collections.ObjectModel;
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
			this.InitializeComponent();
        }

        public void UpdateDay()
        {
            DateTextBlock.Text = Day.Date.ToString();

            EventsStackPanel.Children.Clear();

            foreach (DayEvent dayEvent in Day.Events)
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
    }
}
