
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

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

    public static void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // For pretty printing
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string jsonString = JsonSerializer.Serialize(Timers, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static void LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            Timers = JsonSerializer.Deserialize<ObservableCollection<Timer>>(jsonString);
        }
    }
}
