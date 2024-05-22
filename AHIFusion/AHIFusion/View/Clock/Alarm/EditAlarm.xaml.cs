using System;
using System.Collections.Generic;
using AHIFusion.Model;
namespace AHIFusion;

public sealed partial class EditAlarm : ContentDialog
{
    public string NameEdit { get; set; }
    public TimeSpan TimeEdit { get; set; } = new TimeSpan();
    public Dictionary<string, bool> DaysEdit { get; set; }

    public Alarm alarmEdit;

    public EditAlarm(Alarm alarm)
	{
		this.InitializeComponent();
        alarmEdit = alarm;
        NameEdit = alarm.Title;
        TimeEdit = alarm.Time.ToTimeSpan();
        DaysEdit = new Dictionary<string, bool>(alarm.Days);
	}

	private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
	{
        alarmEdit.Title = NameEdit;
        alarmEdit.Time = TimeOnly.FromTimeSpan(TimeEdit);
        alarmEdit.Days = DaysEdit;
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < AlarmCollection.Alarms.Count; i++)
        {
            if (alarmEdit == AlarmCollection.Alarms[i])
            {
                AlarmCollection.Alarms.RemoveAt(i);
                break;
            }
        }

        this.Hide();
    }
}
