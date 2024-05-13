using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public static DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeOnly), typeof(AlarmControl), new PropertyMetadata(new TimeOnly(0,15), OnTimeChanged));

    public TimeOnly Time
    {
        get { return (TimeOnly)GetValue(TimeProperty); }
        set
        {
            SetValue(TimeProperty, value);
        }
    }

    private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        AlarmControl control = d as AlarmControl;
        control?.UpdateClockHands();
    }
}
