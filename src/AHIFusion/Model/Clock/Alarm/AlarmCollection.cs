using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Text.Json;
using Serilog;

namespace AHIFusion.Model;
public static class AlarmCollection
{
    public static ObservableCollection<Alarm> Alarms { get; set; } = new ObservableCollection<Alarm>();

    public static void Add(Alarm alarm)
    {
        try
        {
            Alarms.Add(alarm);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }

    }
    public static void Remove(Alarm alarm)
    {
        try
        {
            Alarms.Remove(alarm);
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
            Log.Information($"Saving AlarmCollection to file: {filePath}");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(Alarms, options);
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
            Log.Information($"Loading AlarmCollection from file: {filePath}");

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                Alarms = JsonSerializer.Deserialize<ObservableCollection<Alarm>>(jsonString);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
       
    }
}
