using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;
using Windows.ApplicationModel.VoiceCommands;
using Serilog;

namespace AHIFusion.Model;
public static class NoteCollection
{
    public static ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

    public static void Add(Note note)
    {
        Notes.Add(note);
    }
    public static void Remove(Note note)
    {
        Notes.Remove(note);
    }
    public static void SaveToFile(string filePath)
    {
        Log.Information($"Saving NoteCollection to file (File Path: {filePath})");

        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // For pretty printing
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string jsonString = JsonSerializer.Serialize(Notes, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static void LoadFromFile(string filePath)
    {
        Log.Information($"Loading NoteCollection from file (File Path: {filePath})");

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            Notes = JsonSerializer.Deserialize<ObservableCollection<Note>>(jsonString);
        } else
        {
            Log.Information($"File '{filePath}' doesn't exist");
        }
    }
}
