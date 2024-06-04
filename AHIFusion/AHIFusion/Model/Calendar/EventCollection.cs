using System.Collections.ObjectModel;

namespace AHIFusion
{
    class EventCollection
    {
        public static ObservableCollection<DayEvent> Events { get; set; } = new ObservableCollection<DayEvent>();

        public static void Add(DayEvent Event)
        {
            Events.Add(Event);
        }
        public static void Remove(DayEvent Event)
        {
            Events.Remove(Event);
        }

    }
}
