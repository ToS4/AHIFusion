using Windows.UI.Core;

namespace AHIFusion
{
	public sealed partial class CalendarDayControl : UserControl
	{
		public CalendarDayControl()
		{
			this.InitializeComponent();
        }

        public static readonly DependencyProperty DaySelectedProperty = DependencyProperty.Register("DaySelected", typeof(bool), typeof(ShowEventControl), new PropertyMetadata(false));
        public bool DaySelected
        {
            get { return (bool)GetValue(DaySelectedProperty); }
            set { SetValue(DaySelectedProperty, value); }
        }
    }
}
