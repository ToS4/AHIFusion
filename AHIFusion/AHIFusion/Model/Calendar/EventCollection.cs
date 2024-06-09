using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Text.Json;
using Serilog;

namespace AHIFusion
{
    class EventCollection
    {
        public static ObservableCollection<DayEvent> Events { get; set; } = new ObservableCollection<DayEvent>();

        public static void Add(DayEvent Event)
        {
            try
            {
                Events.Add(Event);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }
        public static void Remove(DayEvent Event)
        {
            try
            {
                Events.Remove(Event);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        public static void SaveToFile(string filePath)
        {
            try
            {
                Log.Information($"Saving EventCollection to file: {filePath}");

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // For pretty printing
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                string jsonString = JsonSerializer.Serialize(Events, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
            
        }

        public static void LoadFromFile(string filePath)
        {
            try
            {
                Log.Information($"Loading EventCollection from file: {filePath}");

                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    Events = JsonSerializer.Deserialize<ObservableCollection<DayEvent>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
            
        }

    }
}
