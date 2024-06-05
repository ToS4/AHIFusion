using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using Uno.Extensions.ValueType;
using Windows.Devices.Display.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AHIFusion;
public sealed partial class StopwatchControl : UserControl
{
    DispatcherTimer dispatcherTimer = new DispatcherTimer();
    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    public ObservableCollection<string> LapList = new ObservableCollection<string>();

    public StopwatchControl()
    {
        this.InitializeComponent();
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        LapList.CollectionChanged += LapList_CollectionChanged;
        ResetTextBlocks();
    }

    private void LapList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        LapListView.Items.Clear();
        double flagGridHeight = FlagGrid.ActualHeight;

        for (int i = 0; i < LapList.Count; i++)
        {
            List<string> parts = LapList[i].Split(";").ToList();
            // "1", "00:00:20,35", "00:01:53:81"

            string countString = parts[0];
            string timeString = parts[1];
            string totalString = parts[2];

            TextBlock countTextBlock = new TextBlock();
            countTextBlock.Text = countString;
            countTextBlock.FontSize = 18;
            countTextBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            countTextBlock.Margin = new Thickness(22, 0, 0, 0);

            TextBlock timeTextBlock = new TextBlock();
            timeTextBlock.Text = timeString;
            timeTextBlock.FontSize = 18;
            timeTextBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            timeTextBlock.Margin = new Thickness(22, 0, 0, 0);

            TextBlock totalTextBlock = new TextBlock();
            totalTextBlock.Text = totalString;
            totalTextBlock.FontSize = 18;
            totalTextBlock.Margin = new Thickness(22, 0, 0, 0);
            totalTextBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.6, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(countTextBlock);
            grid.Children.Add(timeTextBlock);
            grid.Children.Add(totalTextBlock);

            Grid.SetColumn(countTextBlock, 0);
            Grid.SetColumn(timeTextBlock, 1);
            Grid.SetColumn(totalTextBlock, 2);

            LapListView.Items.Insert(0, grid);
        }
    }

    private void DispatcherTimer_Tick(object? sender, object e)
    {
        if (stopwatch.IsRunning)
        {
            ElapsedTime = stopwatch.Elapsed;

            hourTextBlock.Text = ElapsedTime.Hours.ToString("D2");
            minuteTextBlock.Text = ElapsedTime.Minutes.ToString("D2");
            secondTextBlock.Text = ElapsedTime.Seconds.ToString("D2");
            msTextBlock.Text = ElapsedTime.Milliseconds.ToString("D3").Substring(0,2);
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
            IsRunning = true;
            StartStopButton.Content = "Stop";
        }
        else
        {
            stopwatch.Stop();
            dispatcherTimer.Stop();
            IsRunning = false;
            StartStopButton.Content = "Start";
        }

    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        stopwatch.Reset();
        dispatcherTimer.Stop();
        StartStopButton.Content = "Start";
        LapList.Clear();
        ElapsedTime = new TimeSpan(0, 0, 0, 0, 0);
        ResetTextBlocks();
    }

    private void ResetTextBlocks()
    {
        hourTextBlock.Text = "00";
        minuteTextBlock.Text = "00";
        secondTextBlock.Text = "00";
        msTextBlock.Text = "00";
    }

    private void FlagButton_Click(object sender, RoutedEventArgs e)
    {
        if (!stopwatch.IsRunning)
        {
            return;
        }
        TimeSpan flagTime = new TimeSpan(0, 0, 0, 0, 0);

        if (LapList.Count == 0)
        {
            flagTime = ElapsedTime;
        }
        else
        {
            TimeSpan totalFlagTime = new TimeSpan(0, 0, 0, 0, 0);
            for (int i = 0; i < LapList.Count; i++)
            {
                List<string> parts = LapList[i].Split(";").ToList();
                // "1", "00:00:20,35", "00:01:53:81"

                string format = @"hh\:mm\:ss\,ff";
                TimeSpan tmp = TimeSpan.ParseExact(parts[1], format, null);
                totalFlagTime = totalFlagTime.Add(tmp);
            }

            flagTime = ElapsedTime.Subtract(totalFlagTime);
        }

        string countString = $"{LapList.Count + 1}";
        string timeString = $"{flagTime.Hours.ToString("D2")}:" +
            $"{flagTime.Minutes.ToString("D2")}:" +
            $"{flagTime.Seconds.ToString("D2")}," +
            $"{flagTime.Milliseconds.ToString("D3").Substring(0,2)}";
        string totalString = $"{ElapsedTime.Hours.ToString("D2")}:" +
            $"{ElapsedTime.Minutes.ToString("D2")}:" +
            $"{ElapsedTime.Seconds.ToString("D2")}," +
            $"{ElapsedTime.Milliseconds.ToString("D3").Substring(0, 2)}";

        LapList.Add($"{countString};{timeString};{totalString}");
    }
}
