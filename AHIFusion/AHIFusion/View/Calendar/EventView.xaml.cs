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
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AHIFusion;
public sealed partial class EventView : ContentDialog
{
    public DayEvent Event;
    public DateOnlyToDateTimeOffsetConverter DateTimeOffsetConverter;
    public EventView()
    {
        this.InitializeComponent();

        Event = new DayEvent()
        {
            Title = "New Event",
            Date = DateOnly.FromDateTime(DateTime.Today)
        };
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        string EventTitle = EventTextBox.Text;
        DateOnly EventDate = DateOnly.FromDateTime(EventDatePicker.SelectedDate.Value.Date);
        Event.Title = EventTitle;
        Event.Date = EventDate;
    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
    }
}
