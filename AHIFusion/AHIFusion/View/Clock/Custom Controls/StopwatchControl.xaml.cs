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
using Windows.Devices.Display.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AHIFusion;
public sealed partial class StopwatchControl : UserControl
{
    DispatcherTimer dispatcherTimer = new DispatcherTimer();
    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    public StopwatchControl()
    {
        this.InitializeComponent();
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
    }

    private void DispatcherTimer_Tick(object? sender, object e)
    {
        if (stopwatch.IsRunning)
        {
            ElapsedTime = stopwatch.Elapsed;
        }
    }

    public static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register("StartTime", typeof(DateTime), typeof(StopwatchControl), new PropertyMetadata(0));
    public DateTime StartTime
    {
        get { return (DateTime)GetValue(StartTimeProperty); }
        set { SetValue(StartTimeProperty, value); }
    }

    public static readonly DependencyProperty ElapsedTimeProperty = DependencyProperty.Register("ElapsedTime", typeof(TimeSpan), typeof(StopwatchControl), new PropertyMetadata(new TimeSpan(0, 0, 0)));
    public TimeSpan ElapsedTime
    {
        get { return (TimeSpan)GetValue(ElapsedTimeProperty); }
        set { SetValue(ElapsedTimeProperty, value); }
    }

    public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning", typeof(bool), typeof(StopwatchControl), new PropertyMetadata(false));
    public bool IsRunning
    {
        get { return (bool)GetValue(IsRunningProperty); }
        set { SetValue(IsRunningProperty, value); }
    }

    private void StartStopButton_Click(object sender, RoutedEventArgs e)
    {
        if (!stopwatch.IsRunning)
        {
            stopwatch.Start();
            dispatcherTimer.Start();
            StartStopButton.Content = "Stop";
        }
        else
        {
            stopwatch.Stop();
            dispatcherTimer.Stop();
            StartStopButton.Content = "Start";
        }

    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        stopwatch.Reset();
        ElapsedTime = new TimeSpan(0, 0, 0, 0, 0);
    }
}
