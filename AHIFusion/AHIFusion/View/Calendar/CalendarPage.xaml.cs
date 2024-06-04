using System;
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

            DisplayEventsList();
        }
    }

    private async void ShowEventControl_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        ShowEventControl showEventControl = sender as ShowEventControl;

        if (showEventControl != null)
        {
            EventView eventView = new EventView()
            {
                Event = showEventControl.Event,
            };

            eventView.XamlRoot = this.XamlRoot;
            await eventView.ShowAsync();

            DisplayEventsList();

            foreach (var gridObject in CalendarGrid.Children)
            {
                CalendarDayControl dayControl = gridObject as CalendarDayControl;

                if (dayControl != null)
                {
                    dayControl.UpdateDay();
                }
            }
        }
    }

    public CalendarPage()
    {
        InitializeComponent();

        _currentDay = DateOnly.FromDateTime(DateTime.Now);
        _currentMonth = _currentDay.AddDays(-_currentDay.Day + 1);
        DisplayCurrentMonth();
    }

    public void DisplayEventsList()
    {
        CurrentDayDateTextBlock.Text = _currentDayControl.Day.Date.ToString();

        CurrentDayEventsStackPanel.Children.Clear();

        foreach (DayEvent dayEvent in EventCollection.Events)
        {

            if (dayEvent.Date == _currentDayControl.Day.Date)
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
            SmallCalendarView.SelectedDates.Clear();

            DateTimeOffset date = new DateTimeOffset(new DateTime(dayControl.Day.Date, TimeOnly.MinValue));
            SmallCalendarView.SelectedDates.Add(date);
            SmallCalendarView.SetDisplayDate(date);
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
