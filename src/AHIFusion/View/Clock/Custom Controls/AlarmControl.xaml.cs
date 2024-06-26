using System;
using System.Collections.Generic;
using System.ComponentModel;
using AHIFusion.Model;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Serilog;

namespace AHIFusion;
public sealed partial class AlarmControl : UserControl
{
    public TimeSpan TimeLeft { get; set; }
    public DispatcherTimer timer = new DispatcherTimer();
    private bool hasRung = false;
    private MediaPlayer _mediaPlayer;

    public AlarmControl()
    {
        try
        {
            Log.Information("Initializing AlarmControl");

            this.InitializeComponent();

            _mediaPlayer = new MediaPlayer();
            AlarmSoundPlayer.SetMediaPlayer(_mediaPlayer);

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();


            UpdateClockHands();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    private void Timer_Tick(object? sender, object e)
    {
        try
        {
            Log.Information("Timer_Tick has been called");

            CalculateTimeLeft();
            if (TimeLeft <= TimeSpan.FromMinutes(1))
            {
                RingAlarm();
                if (!hasRung)
                {
                    ShowNotification(Title, "The alarm is ringing!");
                    hasRung = true;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    private void UpdateClockHands()
    {
        try
        {
            Log.Information("UpdateClockHands has been called");

            TimeOnly now = Time;

            double minuteAngle = now.Minute * 6;
            double hourAngle = (now.Hour % 12 + now.Minute / 60.0) * 30;

            MinuteHand.X2 = 150 + 90 * Math.Sin(minuteAngle * Math.PI / 180);
            MinuteHand.Y2 = 150 - 90 * Math.Cos(minuteAngle * Math.PI / 180);

            HourHand.X2 = 150 + 50 * Math.Sin(hourAngle * Math.PI / 180);
            HourHand.Y2 = 150 - 50 * Math.Cos(hourAngle * Math.PI / 180);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(AlarmControl), new PropertyMetadata(""));

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TimeLeftTextProperty = DependencyProperty.Register("TimeLeftText", typeof(string), typeof(AlarmControl), new PropertyMetadata(""));

    public string TimeLeftText
    {
        get { return (string)GetValue(TimeLeftTextProperty); }
        set { SetValue(TimeLeftTextProperty, value); }
    }

    public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register("IsOn", typeof(bool), typeof(AlarmControl), new PropertyMetadata(true));

    public bool IsOn
    {
        get { return (bool)GetValue(IsOnProperty); }
        set { SetValue(IsOnProperty, value); }
    }

    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeOnly), typeof(AlarmControl), new PropertyMetadata(new TimeOnly(0,0), OnTimeChanged));

    public TimeOnly Time
    {
        get { return (TimeOnly)GetValue(TimeProperty); }
        set
        {
            SetValue(TimeProperty, value);
        }
    }

    public static Dictionary<string, bool> defaultDays = new Dictionary<string, bool>
    {
        { "Mo", false },
        { "Tu", false },
        { "We", true },
        { "Th", false },
        { "Fr", false },
        { "Sa", false },
        { "Su", false }
    };
    public static readonly DependencyProperty DaysProperty = DependencyProperty.Register("Days", typeof(Dictionary<string, bool>), typeof(AlarmControl), new PropertyMetadata(defaultDays));
    public Dictionary<string, bool> Days
    {
        get { return (Dictionary<string, bool>)GetValue(DaysProperty); }
        set { SetValue(DaysProperty, value); }
    }

    public static readonly DependencyProperty SoundProperty = DependencyProperty.Register("Sound", typeof(string), typeof(AlarmControl), new PropertyMetadata(""));
    public string Sound
    {
        get { return (string)GetValue(SoundProperty); }
        set { SetValue(SoundProperty, value); }
    }

    public void RingAlarm()
    {

        _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri($"ms-appx:///Assets/Sounds/{Sound}"));

        _mediaPlayer.Play();
    }

    private void ShowNotification(string title, string content)
    {
        try
        {
            Log.Information("ShowNotification has been called");

            Log.Debug("Create the toast notification content & notification must be enabled in windows settings");

            var toastContent = new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddText(title)
                .AddText(content)
                .GetToastContent();

            var toastNotification = new ToastNotification(toastContent.GetXml());

            if (IsOn)
            {
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }

            Log.Debug("Notification should be shown now.");
        }
        catch (System.Runtime.InteropServices.COMException comEx)
        {
            Log.Error($"COMException: {comEx}");
        }
        catch (Exception ex)
        {
            Log.Error($"Exception: {ex.Message}");
        }
    }

    private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        AlarmControl control = d as AlarmControl;
        control?.UpdateClockHands();
    }

    private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        ClockViewbox.Visibility = Visibility.Visible;
        TextViewbox.Visibility = Visibility.Collapsed;
        rect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 149, 149, 149));
    }

    private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        ClockViewbox.Visibility = Visibility.Collapsed;
        TextViewbox.Visibility = Visibility.Visible;
        rect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 169, 169, 169));
    }

    private void CalculateTimeLeft()
    {
        try
        {
            Log.Information("CalculateTimeLeft has been called");

            DateTime now = DateTime.Now;
            DateTime alarmTimeToday = new DateTime(now.Year, now.Month, now.Day, Time.Hour, Time.Minute, 0);

            if (now > alarmTimeToday)
            {
                alarmTimeToday = alarmTimeToday.AddDays(1);
            }

            if (IsOn == false)
            {
                TimeLeft = TimeSpan.FromDays(7);
                TimeLeftText = "Alarm is off";
                hasRung = false;
                return;
            }

            for (int i = 0; i < 7; i++)
            {
                string dayOfWeek = alarmTimeToday.DayOfWeek.ToString().Substring(0, 2);
                if (Days.ContainsKey(dayOfWeek) && Days[dayOfWeek])
                {
                    TimeLeft = alarmTimeToday - DateTime.Now;
                    if (TimeLeft.Days != 0)
                    {
                        TimeLeftText = $"Your Alarm will ring in {TimeLeft.Days}d {TimeLeft.Hours}h {TimeLeft.Minutes}min";
                        if (TimeLeft > TimeSpan.FromMinutes(1))
                        {
                            hasRung = false;
                        }
                        return;
                    }
                    TimeLeftText = $"Your Alarm will ring in {TimeLeft.Hours}h {TimeLeft.Minutes}min";
                    if (TimeLeft > TimeSpan.FromMinutes(1))
                    {
                        hasRung = false;
                    }
                    return;
                }

                alarmTimeToday = alarmTimeToday.AddDays(1);
            }

            TimeLeft = TimeSpan.FromDays(7);
            TimeLeftText = "No day selected";
            hasRung = false;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }

    }

    private async void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        try
        {
            Log.Information("Grid_PointerPressed has been called");

            Log.Debug("Get the Alarm associated with this AlarmControl");
            Alarm alarm = this.DataContext as Alarm;

            if (alarm != null)
            {
                Log.Debug("Create a new EditAlarm dialog");
                EditAlarm editAlarmDialog = new EditAlarm(alarm);

                Log.Debug("Show the EditAlarm dialog");
                editAlarmDialog.XamlRoot = this.XamlRoot;
                await editAlarmDialog.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }
}
