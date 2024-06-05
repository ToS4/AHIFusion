using System.Collections.Specialized;
using AHIFusion.Model;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Uno;

namespace AHIFusion;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class StopwatchContent : Page
{
    public StopwatchContent()
    {
        this.InitializeComponent();
        SetStopwatch();
    }

    private void SetStopwatch()
    {
        MainGrid.Children.Clear();
        StopwatchControl stopwatchControl = new StopwatchControl()
        {
            DataContext = StopwatchCollection.Stopwatches[0]
        };

        Binding startTimeBinding = new Binding
        {
            Path = new PropertyPath("StartTime"),
            Mode = BindingMode.TwoWay
        };

        Binding elapsedTimeBinding = new Binding
        {
            Path = new PropertyPath("ElapsedTime"),
            Mode = BindingMode.TwoWay
        };

        Binding isRunningBinding = new Binding
        {
            Path = new PropertyPath("IsRunning"),
            Mode = BindingMode.TwoWay
        };

        stopwatchControl.SetBinding(StopwatchControl.StartTimeProperty, startTimeBinding);
        stopwatchControl.SetBinding(StopwatchControl.ElapsedTimeProperty, elapsedTimeBinding);
        stopwatchControl.SetBinding(StopwatchControl.IsRunningProperty, isRunningBinding);

        if (StopwatchCollection.Stopwatches[0].IsRunning)
        {
            stopwatchControl.dispatcherTimer.Start();
        }
        MainGrid.Children.Add(stopwatchControl);
    }
}
