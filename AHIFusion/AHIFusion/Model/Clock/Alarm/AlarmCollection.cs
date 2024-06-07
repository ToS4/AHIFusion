using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Text.Json;

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

    public static void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // For pretty printing
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string jsonString = JsonSerializer.Serialize(Alarms, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static void LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            Alarms = JsonSerializer.Deserialize<ObservableCollection<Alarm>>(jsonString);
        }
    }
}
