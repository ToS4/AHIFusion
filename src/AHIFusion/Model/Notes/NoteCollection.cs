using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;
using Windows.ApplicationModel.VoiceCommands;
using Serilog;
using Uno;

namespace AHIFusion.Model;
public static class NoteCollection
{
    public static ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

    public static void Add(Note note)
    {
        try
        {
            Notes.Add(note);
        }
        catch (Exception ex) 
        {
            Log.Error(ex, "An error occurred");
        }
    }
    public static void Remove(Note note)
    {
        try
        {
            Notes.Remove(note);
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
            Log.Information($"Saving NoteCollection to file: {filePath}");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(Notes, options);
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
            Log.Information($"Loading NoteCollection from file: {filePath}");

            if (File.Exists(filePath))
            {
                Log.Debug($"File '{filePath}' exists");
                string jsonString = File.ReadAllText(filePath);
                Notes = JsonSerializer.Deserialize<ObservableCollection<Note>>(jsonString);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }
}
