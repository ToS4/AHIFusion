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

    public async static void SaveToFile(string filePath)
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

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("AppData", CreationCollisionOption.OpenIfExists);
            File.WriteAllText(Path.Combine(folder.Path, filePath), jsonString);

        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
        
    }

    public async  static void LoadFromFile(string filePath)
    {
        try
        {
            Log.Information($"Loading AlarmCollection from file: {filePath}");

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("AppData", CreationCollisionOption.OpenIfExists);
            var newPath = Path.Combine(folder.Path, filePath);

            if (File.Exists(newPath))
            {
                string jsonString = File.ReadAllText(newPath);
                Alarms = JsonSerializer.Deserialize<ObservableCollection<Alarm>>(jsonString);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
       
    }
}
