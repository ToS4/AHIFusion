using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI.Popups;
namespace AHIFusion;
public sealed partial class AlarmControl : UserControl
{

    public AlarmControl()
    {
        this.InitializeComponent();

        UpdateClockHands();
    }

    private void UpdateClockHands()
    {
        TimeOnly now = Time;
        double minuteAngle = now.Minute * 6; // 360 degrees / 60 minutes = 6 degrees per minute
        double hourAngle = (now.Hour % 12 + now.Minute / 60.0) * 30; // 360 degrees / 12 hours = 30 degrees per hour

        MinuteHand.X2 = 150 + 90 * Math.Sin(minuteAngle * Math.PI / 180);
        MinuteHand.Y2 = 150 - 90 * Math.Cos(minuteAngle * Math.PI / 180);

        HourHand.X2 = 150 + 50 * Math.Sin(hourAngle * Math.PI / 180);
        HourHand.Y2 = 150 - 50 * Math.Cos(hourAngle * Math.PI / 180);
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(AlarmControl), new PropertyMetadata(""));

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register("IsOn", typeof(bool), typeof(AlarmControl), new PropertyMetadata(true));

    public bool IsOn
    {
        get { return (bool)GetValue(IsOnProperty); }
        set { SetValue(IsOnProperty, value); }
    }

    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeOnly), typeof(AlarmControl), new PropertyMetadata(new TimeOnly(0,15), OnTimeChanged));

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
}
