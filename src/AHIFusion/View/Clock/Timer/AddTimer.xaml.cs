using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AHIFusion.Model;
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
public sealed partial class AddTimer : ContentDialog
{
    public string NameAdd { get; set; } = $"Timer {TimerCollection.Timers.Count + 1}";
    public TimeSpan TimeAdd { get; set; } = new TimeSpan(0, 0, 0);

    public AddTimer()
    {
        this.InitializeComponent();
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        try
        {
            Log.Information("ContentDialog_PrimaryButtonClick has been called");

            TimeAdd = CustomTimePicker.Time;

            if (TimeAdd.TotalSeconds == 0)
            {
                return;
            }

            AHIFusion.Model.Timer timerToAdd = new AHIFusion.Model.Timer()
            {
                Title = NameAdd,
                Time = TimeAdd,
                InitialTime = TimeAdd.TotalSeconds,
                IsRunning = false
            };
            TimerCollection.Timers.Add(timerToAdd);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }
}
