using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AHIFusion.Model;

namespace AHIFusion;
public static class TodoCollection
{
    public static ObservableCollection<TodoList> TodoLists { get; set; } = new ObservableCollection<TodoList>();

    public static void Add(TodoList todoList)
    {
        TodoLists.Add(todoList);
    }

    public static void Remove(TodoList todoList)
    {
        TodoLists.Remove(todoList);
    }

    public static void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // For pretty printing
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        string jsonString = JsonSerializer.Serialize(TodoLists, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static void LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            TodoLists = JsonSerializer.Deserialize<ObservableCollection<TodoList>>(jsonString);
        }
    }
}
