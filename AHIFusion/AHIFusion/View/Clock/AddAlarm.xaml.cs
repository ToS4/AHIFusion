using System;
using System.Collections.Generic;
using AHIFusion.Model;
namespace AHIFusion;
public sealed partial class AddAlarm : ContentDialog
{
    public string Name { get; set; } = $"Alarm {AlarmCollection.Alarms.Count + 1}";
    public TimeSpan Time { get; set; } = new TimeSpan(10, 0, 0);

    public AddAlarm()
    {
        this.InitializeComponent();
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        AlarmCollection.Alarms.Add(new Alarm(Name, TimeOnly.FromTimeSpan(Time), true));
    }
}