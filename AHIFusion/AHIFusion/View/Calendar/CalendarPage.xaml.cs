using System.Security.Cryptography.X509Certificates;
using Microsoft.UI;
using static System.Net.Mime.MediaTypeNames;

namespace AHIFusion;

public sealed partial class CalendarPage : Page
{
    private string[] _days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    public List<CalendarDayControl> DayControlList { get; set; } = new List<CalendarDayControl>();

    private DateOnly _currentMonth;
    private DateOnly _currentDay;

    private CalendarDayControl _currentDayControl;
    private CalendarDayControl _CurrentDayControl
    {
        get
        {
            return _currentDayControl;
        }

        set
        {
            if (_currentDayControl != null)
            {
                _currentDayControl.DaySelected = false;
            }

            _currentDayControl = value;
            _currentDayControl.DaySelected = true;

            CurrentDayDateTextBlock.Text = _currentDayControl.Day.Date.ToString();

            CurrentDayEventsStackPanel.Children.Clear();

            foreach (ShowEventControl showEventControl in CurrentDayEventsStackPanel.Children)
            {
                showEventControl.PointerPressed -= ShowEventControl_PointerPressed;
                CurrentDayEventsStackPanel.Children.Remove(showEventControl);
            }

            foreach (DayEvent dayEvent in _currentDayControl.Day.Events)
            {

                ShowEventControl showEventControl = new ShowEventControl()
                {
                    Margin = new Thickness(5),
                    Event = dayEvent
                };

                CurrentDayEventsStackPanel.Children.Add(showEventControl);
                showEventControl.UpdateEvent();

                showEventControl.PointerPressed += ShowEventControl_PointerPressed;
            }
        }
    }

    private void ShowEventControl_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        ShowEventControl showEventControl = sender as ShowEventControl;

        if (showEventControl != null)
        {
            EventView eventView = new EventView()
            {
                Event = showEventControl.Event,
            };

            eventView.XamlRoot = this.XamlRoot;
            eventView.ShowAsync();
        }
    }

    public CalendarPage()
    {
        InitializeComponent();

        _currentDay = DateOnly.FromDateTime(DateTime.Now);
        _currentMonth = _currentDay.AddDays(-_currentDay.Day + 1);
        DisplayCurrentMonth();
    }

    private void DisplayCurrentMonth()
    {
        CalendarGrid.Children.Clear();
        DateOnly firstDayOfMonth = new DateOnly(_currentMonth.Year, _currentMonth.Month, 1);
        int firstDayOffset = (int)firstDayOfMonth.DayOfWeek;
        DateOnly firstDayToDisplay = firstDayOfMonth.AddDays(-firstDayOffset);

        for (int i = 0; i < 7; i++)
        {
            TextBlock textBlock = new TextBlock()
            {
                TextAlignment = TextAlignment.Center,
                Text = _days[i]
            };

            Grid.SetColumn(textBlock, i);

            CalendarGrid.Children.Add(textBlock);
        }

        for (int i = 1; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                DateOnly currentDate = firstDayToDisplay.AddDays((i-1) * 7 + j);
                CalendarDay day = new CalendarDay { Date = currentDate };

                // Populate events for the day
                for (int k = 0; k < 3; k++) // Example: adding 3 dummy events per day
                {
                    DayEvent dayEvent = new DayEvent
                    {
                        Title = $"{k + 1}. Event for {currentDate}",
                        Date = currentDate
                    };
                    day.Events.Add(dayEvent);
                }

                CalendarDayControl dayControl = new CalendarDayControl
                {
                    Padding = new Thickness(10),
                    Day = day
                };

                if (day.Date == _currentDay)
                {
                    _CurrentDayControl = dayControl;
                }

                Grid.SetRow(dayControl, i);
                Grid.SetColumn(dayControl, j);

                CalendarGrid.Children.Add(dayControl);
                dayControl.UpdateDay();

                dayControl.PointerPressed += DayControl_PointerPressed;
            }
        }

        //MonthYearDisplay.Text = $"{_currentMonth.ToString("MMMM yyyy")}";
    }

    private void DayControl_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        CalendarDayControl dayControl = sender as CalendarDayControl;
        if (dayControl != null)
        {
            _CurrentDayControl = dayControl;
        }
    }

    private void SmallCalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
    {

        if (SmallCalendarView.SelectedDates.Count > 0)
        {
            DateTimeOffset dateTimeOffset = SmallCalendarView.SelectedDates[0];
            if (dateTimeOffset != null)
            {
                _currentDay = DateOnly.FromDateTime(dateTimeOffset.DateTime);
                _currentMonth = _currentDay.AddDays(-_currentDay.Day + 1);
                DisplayCurrentMonth();
            }
        }


    }
}
