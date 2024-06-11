using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AHIFusion.Model;
using Serilog;

namespace AHIFusion;
public static class TodoCollection
{
    public static ObservableCollection<TodoList> TodoLists { get; set; } = new ObservableCollection<TodoList>();

    public static void Add(TodoList todoList)
    {
        try
        {
            TodoLists.Add(todoList);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
        
    }

    public static void Remove(TodoList todoList)
    {
        try
        {
            TodoLists.Remove(todoList);
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
            Log.Information($"Saving TodoCollection to file: {filePath}");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For pretty printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string jsonString = JsonSerializer.Serialize(TodoLists, options);

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
            Log.Information($"Loading TodoCollection from file: {filePath}");

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync("AppData", CreationCollisionOption.OpenIfExists);
            var newPath = Path.Combine(folder.Path, filePath);

            if (File.Exists(newPath))
            {
                string jsonString = File.ReadAllText(newPath);
                TodoLists = JsonSerializer.Deserialize<ObservableCollection<TodoList>>(jsonString);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
        
    }
}
