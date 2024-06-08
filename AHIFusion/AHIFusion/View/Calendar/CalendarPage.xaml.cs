using System;
using System.Security.Cryptography.X509Certificates;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.UI;
using Microsoft.UI.Text;
using Serilog;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        try
        {
            ShowEventControl showEventControl = sender as ShowEventControl;

            if (showEventControl != null)
            {
                EventView eventView = new EventView(false)
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
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling ShowEventControl_PointerPressed click event");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public CalendarPage()
    {
        try
        {
            Log.Information("Initializing CalendarPage");

            InitializeComponent();

            _currentDay = DateOnly.FromDateTime(DateTime.Now);
            _currentMonth = _currentDay.AddDays(-_currentDay.Day + 1);

            Log.Information("Initialized CalendarPage with current day: {currentDay} and current month: {currentMonth}", _currentDay, _currentMonth);

            DisplayCurrentMonth();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while initializing CalendarPage");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }

    public void DisplayEventsList()
    {
        try
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
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while displaying events list");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        
    }

    private void DisplayCurrentMonth()
    {
        try
        {
            Log.Information("Displaying current month: {currentMonth}", _currentMonth);

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
                    DateOnly currentDate = firstDayToDisplay.AddDays((i - 1) * 7 + j);
                    CalendarDay day = new CalendarDay { Date = currentDate };

                    CalendarDayControl dayControl = new CalendarDayControl
                    {
                        Padding = new Thickness(10),
                        Day = day
                    };

                    if (day.Date == _currentDay)
                    {
                        Log.Information("Setting _CurrentDayControl for current day: {currentDay}", _currentDay);
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
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while displaying current month list");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }   
    }

    private void DayControl_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        try
        {
            Log.Information("DayControl_PointerPressed event triggered");

            CalendarDayControl dayControl = sender as CalendarDayControl;
            if (dayControl != null)
            {
                SmallCalendarView.SelectedDates.Clear();

                DateTimeOffset date = new DateTimeOffset(new DateTime(dayControl.Day.Date, TimeOnly.MinValue));
                SmallCalendarView.SelectedDates.Add(date);
                SmallCalendarView.SetDisplayDate(date);

                Log.Information("Date: {date}", date);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling DayControl click event");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private void SmallCalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
    {
        try
        {
            Log.Information("SmallCalendarView_SelectedDatesChanged event triggered");

            if (SmallCalendarView.SelectedDates.Count > 0)
            {
                DateTimeOffset dateTimeOffset = SmallCalendarView.SelectedDates[0];
                if (dateTimeOffset != null)
                {
                    Log.Information("Selected date: {dateTimeOffset}", dateTimeOffset);

                    _currentDay = DateOnly.FromDateTime(dateTimeOffset.DateTime);
                    _currentMonth = _currentDay.AddDays(-_currentDay.Day + 1);
                    DisplayCurrentMonth();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling SmallCalendarView selection changed event");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("AddButton_Click event triggered");

            EventView eventView = new EventView(true, _currentDayControl.Day.Date);

            eventView.XamlRoot = this.XamlRoot;
            await eventView.ShowAsync();

            DisplayCurrentMonth();
            //DisplayEventsList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling AddButton click event");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }

    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {  
        try
        {
            Log.Information("Page_Loaded event triggered");

            if (_currentDay != null && _currentMonth != null)
            {
                DisplayCurrentMonth();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while loading Page");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("SaveButton_Click event triggered");

            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("iCalendar file", new List<string>() { ".ics" });
            savePicker.SuggestedFileName = "Calendar";

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            Log.Information($"Selecting file");
            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                Log.Information($"File selected, Path: {file.Path}");

                var calendar = new Calendar();

                foreach (var dayEvent in EventCollection.Events)
                {
                    var calendarEvent = new CalendarEvent
                    {
                        Summary = dayEvent.Title,
                        Start = new CalDateTime(dayEvent.Date.ToDateTime(TimeOnly.MinValue), "UTC"),
                        IsAllDay = true,
                        Uid = Guid.NewGuid().ToString(),
                        DtStamp = new CalDateTime(DateTime.UtcNow)
                    };
                    calendar.Events.Add(calendarEvent);
                }

                var serializer = new CalendarSerializer();
                var serializedCalendar = serializer.SerializeToString(calendar);

                await FileIO.WriteTextAsync(file, serializedCalendar);
            }
            else
            {
                Log.Information($"File has not been selected");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling SaveButton click event");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        
    }

    private async void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("LoadButton_Click event triggered");

            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".ics");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(open, hwnd);

            Log.Information($"Selecting file");
            StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                Log.Information($"File selected, Path: {file.Path}");
                using (IRandomAccessStream randAccStream =
                    await file.OpenAsync(FileAccessMode.Read))
                {
                    using (Stream stream = randAccStream.AsStreamForRead())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string icsContent = await reader.ReadToEndAsync();
                        var calendar = Calendar.Load(icsContent);

                        foreach (var calendarEvent in calendar.Events)
                        {
                            string title = calendarEvent.Summary;
                            DateOnly date = DateOnly.FromDateTime(calendarEvent.Start.AsDateTimeOffset.DateTime);

                            DayEvent dayEvent = new DayEvent()
                            {
                                Title = title,
                                Date = date
                            };

                            EventCollection.Add(dayEvent);
                        }
                    }
                }
            }
            else
            {
                Log.Information($"File has not been selected");
            }

            DisplayCurrentMonth();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling LoadButton click event");
            throw new ArgumentException("Error, please check logs!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
