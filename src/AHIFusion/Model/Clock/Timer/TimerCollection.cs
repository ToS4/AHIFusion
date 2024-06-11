
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using Serilog;

namespace AHIFusion.Model;

public static class TimerCollection
{
    public static ObservableCollection<Timer> Timers { get; set; } = new ObservableCollection<Timer>();

    public static void Add(Timer timer)
    {
        try
        {
            Timers.Add(timer);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
       
    }
    public static void Remove(Timer timer)
    {
        try
        {
            Timers.Remove(timer);
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
            Log.Information($"Saving TimerCollection to file: {filePath}");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(Timers, options);

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("AppData", CreationCollisionOption.OpenIfExists);
            File.WriteAllText(Path.Combine(folder.Path, filePath), jsonString);

        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
        
    }

    public async static void LoadFromFile(string filePath)
    {
        try
        {
            Log.Information($"Loading TimerCollection from file: {filePath}");

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("AppData", CreationCollisionOption.OpenIfExists);
            var newPath = Path.Combine(folder.Path, filePath);

            if (File.Exists(newPath))
            {
                string jsonString = File.ReadAllText(newPath);
                Timers = JsonSerializer.Deserialize<ObservableCollection<Timer>>(jsonString);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
       
    }
}
