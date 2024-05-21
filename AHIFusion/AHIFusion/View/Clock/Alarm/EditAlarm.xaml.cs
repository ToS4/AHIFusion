using System;
using System.Collections.Generic;
using AHIFusion.Model;
namespace AHIFusion;

public sealed partial class EditAlarm : ContentDialog
{
    public string NameEdit { get; set; }
    public TimeSpan TimeEdit { get; set; } = new TimeSpan();
    public Dictionary<string, bool> DaysEdit { get; set; } = new Dictionary<string, bool>();

    public Alarm alarmEdit;

    public EditAlarm(Alarm alarm)
	{
		this.InitializeComponent();
        alarmEdit = alarm;
        NameEdit = alarm.Title;
        TimeEdit = alarm.Time.ToTimeSpan();
        DaysEdit = alarm.Days;
	}

	private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
	{
        alarmEdit.Title = NameEdit;
        alarmEdit.Time = TimeOnly.FromTimeSpan(TimeEdit);
        alarmEdit.Days = DaysEdit; //Das speichert, obwohl man canceled; es updated die UI nicht
    }
}
