
using System.Collections.ObjectModel;

namespace AHIFusion.Model;

public static class TimerCollection
{
    public static ObservableCollection<Timer> Timers { get; set; } = new ObservableCollection<Timer>();

    public static void Add(Timer timer)
    {
        Timers.Add(timer);
    }
    public static void Remove(Timer timer)
    {
        Timers.Remove(timer);
    }
}
