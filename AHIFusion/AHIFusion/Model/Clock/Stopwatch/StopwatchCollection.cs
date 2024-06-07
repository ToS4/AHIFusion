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

namespace AHIFusion;
public static class StopwatchCollection
{
    public static ObservableCollection<Stopwatch> Stopwatches { get; set; } = new ObservableCollection<Stopwatch>();

    public static void Add(Stopwatch stopwatch)
    {
        Stopwatches.Add(stopwatch);
    }

    public static void Remove(Stopwatch stopwatch)
    {
        Stopwatches.Remove(stopwatch);
    }

    public static void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // For pretty printing
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string jsonString = JsonSerializer.Serialize(Stopwatches, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static void LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            Stopwatches = JsonSerializer.Deserialize<ObservableCollection<Stopwatch>>(jsonString);
        }
    }
}
