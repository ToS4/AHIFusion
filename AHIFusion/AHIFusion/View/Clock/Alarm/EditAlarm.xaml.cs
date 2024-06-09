using System;
using System.Collections.Generic;
using AHIFusion.Model;
using Serilog;
namespace AHIFusion;

public sealed partial class EditAlarm : ContentDialog
{
    public string NameEdit { get; set; }
    public TimeSpan TimeEdit { get; set; } = new TimeSpan();
    public Dictionary<string, bool> DaysEdit { get; set; }

    public List<string?> SoundsEdit { get; set; } = Alarm.SoundPaths;
    public string? SelectedSound { get; set; }

    public Alarm alarmEdit;

    public EditAlarm(Alarm alarm)
	{
        try
        {
            Log.Information("Initializing EditAlarm");

            this.InitializeComponent();
            alarmEdit = alarm;
            NameEdit = alarm.Title;
            TimeEdit = alarm.Time.ToTimeSpan();
            DaysEdit = new Dictionary<string, bool>(alarm.Days);
            SelectedSound = alarm.Sound;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }


	}

	private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
	{
        try
        {
            Log.Information("ContentDialog_PrimaryButtonClick has been called");

            alarmEdit.Title = NameEdit;
            alarmEdit.Time = TimeOnly.FromTimeSpan(TimeEdit);
            alarmEdit.Days = DaysEdit;
            alarmEdit.Sound = SelectedSound;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }


    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("DeleteButton_Click has been called");

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
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }
}
