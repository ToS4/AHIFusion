using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Text.Json;

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

        public static void SaveToFile(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(Events, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                Events = JsonSerializer.Deserialize<ObservableCollection<DayEvent>>(jsonString);
            }
        }

    }
}
