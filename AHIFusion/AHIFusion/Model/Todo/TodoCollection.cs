using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
