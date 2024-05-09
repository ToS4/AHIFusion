using System;
using System.Collections.Generic;
namespace AHIFusion;
public sealed partial class AlarmControl : UserControl
{
    private double hourAngle = 0;
    private double minuteAngle = 0;
    private const double HourSpeed = 0.05; // Adjust this value to change hour hand speed
    private const double MinuteSpeed = 0.5;

    public AlarmControl()
    {
        this.InitializeComponent();
        CompositionTarget.Rendering += CompositionTarget_Rendering;
    }

    private void CompositionTarget_Rendering(object? sender, object e)
    {
        DateTime currentTime = DateTime.Now;

        hourAngle += HourSpeed;
        minuteAngle += MinuteSpeed;

        // Apply rotation to hands
        hourHand.RenderTransform = new RotateTransform { Angle = hourAngle, CenterX = 0, CenterY = 115 };
        minuteHand.RenderTransform = new RotateTransform { Angle = minuteAngle, CenterX = 0, CenterY = 115 };
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

    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeOnly), typeof(AlarmControl), new PropertyMetadata(new TimeOnly(0,0,0)));

    public TimeOnly Time
    {
        get { return (TimeOnly)GetValue(TimeProperty); }
        set { SetValue(TimeProperty, value); }
    }
}
