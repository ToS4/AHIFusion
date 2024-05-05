using System.Collections.ObjectModel;

namespace AHIFusion.Model;
public class AlarmCollection
{
    public ObservableCollection<Alarm> Alarms { get; set; } = new ObservableCollection<Alarm>();

    public void Add(Alarm alarm)
    {
        Alarms.Add(alarm);
    }
    public void Remove(Alarm alarm)
    {
        Alarms.Remove(alarm);
    }
}
