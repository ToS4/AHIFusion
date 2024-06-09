using System;
using AHIFusion.Model;
using Serilog;
namespace AHIFusion;
public sealed partial class EditTimer : ContentDialog
{
    public string NameEdit { get; set; }
    public TimeSpan TimeEdit { get; set; } = new TimeSpan();

    public AHIFusion.Model.Timer timerEdit;

    public EditTimer(AHIFusion.Model.Timer timer)
    {
        try
        {
            Log.Information("Initializing EditTimer");

            this.InitializeComponent();
            timerEdit = timer;
            NameEdit = timer.Title;
            TimeEdit = TimeSpan.FromSeconds(timer.Time.TotalSeconds);
            CustomTimePicker.Time = TimeEdit;
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

            TimeEdit = CustomTimePicker.Time;
            timerEdit.Title = NameEdit;
            timerEdit.Time = TimeEdit;
            timerEdit.InitialTime = TimeEdit.TotalSeconds;
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

            for (int i = 0; i < TimerCollection.Timers.Count; i++)
            {
                if (timerEdit == TimerCollection.Timers[i])
                {
                    TimerCollection.Timers.RemoveAt(i);
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
