using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AHIFusion;
public sealed partial class EventView : ContentDialog
{
    public DayEvent Event;
    public DateOnlyToDateTimeOffsetConverter DateTimeOffsetConverter;
    public EventView(bool isNew)
    {
        try
        {
            this.InitializeComponent();

            if (isNew)
            {
                Event = new DayEvent()
                {
                    Date = DateOnly.FromDateTime(DateTime.Today)
                };

                DeleteFirstButton.Visibility = Visibility.Collapsed;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while initializing EventView");

        }
    }

    public EventView(bool isNew, DateOnly givenDate)
    {
        try
        {
            this.InitializeComponent();

            if (isNew)
            {
                Event = new DayEvent()
                {
                    Date = givenDate
                };

                DeleteFirstButton.Visibility = Visibility.Collapsed;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while initializing EventView");

        }
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        try
        {
            string EventTitle = EventTextBox.Text;
            DateOnly EventDate = DateOnly.FromDateTime(EventDatePicker.SelectedDate.Value.Date);

            if (string.IsNullOrEmpty(EventTitle))
            {
                return;
            }

            if (!EventCollection.Events.Contains(Event))
            {
                EventCollection.Add(Event);
            }

            Event.Title = EventTitle;
            Event.Date = EventDate;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling ContentDialog_PrimaryButton click event");

        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (EventCollection.Events.Contains(Event))
            {
                EventCollection.Remove(Event);
                Hide();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while handling DeleteButton click event");

        }
    }
}
