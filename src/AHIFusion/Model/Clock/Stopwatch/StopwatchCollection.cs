using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AHIFusion.Model;
using Ical.Net.CalendarComponents;
using Serilog;

namespace AHIFusion;
public static class StopwatchCollection
{
    public static ObservableCollection<Stopwatch> Stopwatches { get; set; } = new ObservableCollection<Stopwatch>();

    public static void Add(Stopwatch stopwatch)
    {
        try
        {
            Stopwatches.Add(stopwatch);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
       
    }

    public static void Remove(Stopwatch stopwatch)
    {
        try
        {
            Stopwatches.Remove(stopwatch);
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
            Log.Information($"Saving StopwatchCollection to file: {filePath}");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(Stopwatches, options);

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
            Log.Information($"Loading StopwatchCollection from file: {filePath}");

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("AppData", CreationCollisionOption.OpenIfExists);
            var newPath = Path.Combine(folder.Path, filePath);

            if (File.Exists(newPath))
            {
                string jsonString = File.ReadAllText(newPath);
                Stopwatches = JsonSerializer.Deserialize<ObservableCollection<Stopwatch>>(jsonString);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
        
    }
}
