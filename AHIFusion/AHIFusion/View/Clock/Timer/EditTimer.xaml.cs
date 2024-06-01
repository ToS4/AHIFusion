using System;
using AHIFusion.Model;
namespace AHIFusion;
public sealed partial class EditTimer : ContentDialog
{
    public string NameEdit { get; set; }
    public TimeSpan TimeEdit { get; set; } = new TimeSpan();

    public AHIFusion.Model.Timer timerEdit;

    public EditTimer(AHIFusion.Model.Timer timer)
    {
        this.InitializeComponent();
        timerEdit = timer;
        NameEdit = timer.Title;
        TimeEdit = TimeSpan.FromSeconds(timer.Time.TotalSeconds);
        CustomTimePicker.Time = TimeEdit;
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        TimeEdit = CustomTimePicker.Time;
        timerEdit.Title = NameEdit;
        timerEdit.Time = TimeEdit;
        timerEdit.InitialTime = TimeEdit.TotalSeconds;
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
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
}
