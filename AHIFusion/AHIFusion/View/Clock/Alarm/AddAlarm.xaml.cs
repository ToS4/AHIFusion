using System;
using System.Collections.Generic;
using AHIFusion.Model;
using Windows.UI.Core;
using Windows.UI.WindowManagement;

namespace AHIFusion;
public sealed partial class AddAlarm : ContentDialog
{
    public string NameAdd { get; set; } = $"Alarm {AlarmCollection.Alarms.Count + 1}";
    public TimeSpan TimeAdd { get; set; } = new TimeSpan(12, 0, 0);
    public Dictionary<string, bool> DaysAdd { get; set; } = new Dictionary<string, bool>
        {
            { "Mo", true },
            { "Tu", true },
            { "We", true },
            { "Th", true },
            { "Fr", true },
            { "Sa", true },
            { "Su", true }
        };
    public List<string> SoundsAdd { get; set; } = Alarm.SoundPaths;
    public string SelectedSound { get; set; }

    public AddAlarm()
    {
        this.InitializeComponent();
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Alarm alarmToAdd = new Alarm()
        {
            Title = NameAdd,
            Time = TimeOnly.FromTimeSpan(TimeAdd),
            IsOn = true,
            Days = DaysAdd,
            Sound = SelectedSound
        };
        AlarmCollection.Alarms.Add(alarmToAdd);
    }
}
