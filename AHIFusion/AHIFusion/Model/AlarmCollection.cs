using System.Collections.ObjectModel;

namespace AHIFusion.Model;
public static class AlarmCollection
{
    public static ObservableCollection<Alarm> Alarms { get; set; } = new ObservableCollection<Alarm>();

    public static void Add(Alarm alarm)
    {
        Alarms.Add(alarm);
    }
    public static void Remove(Alarm alarm)
    {
        Alarms.Remove(alarm);
    }
}
