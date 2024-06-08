using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AHIFusion;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TodoPage : Page
{

    public TodoPage()
    {
        this.InitializeComponent();

        LoadTodoFromList(TodoCollection.TodoLists);

        TodoCollection.TodoLists.CollectionChanged += TodoLists_CollectionChanged;

        TodoListListView.SelectionChanged += TodoListListView_SelectionChanged;

        TitleTextBlock.Text = TodoListListView.Items.OfType<TodoListControl>().FirstOrDefault()?.Name;
        if (TitleTextBlock.Text == "" || TitleTextBlock.Text is null)
        {
            AddTodoItemButton.Visibility = Visibility.Collapsed;
        }
    }

    private void TodoListListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.RemovedItems.Count > 0 && e.RemovedItems[0] is TodoListControl oldControl)
        {
            oldControl.IsSelected = false;
        }

        if (e.AddedItems.Count > 0 && e.AddedItems[0] is TodoListControl newControl)
        {
            newControl.IsSelected = true;
            TitleTextBlock.Text = newControl.Name;
            UpdateTodoListView(newControl.DataContext as TodoList);

            TitleTextBlockStoryboard.Begin();

            AddTodoItemButton.Visibility = Visibility.Visible;
        }
        else
        {
            AddTodoItemButton.Visibility = Visibility.Collapsed;
        }
    }

    private void LoadTodoFromList(IEnumerable<TodoList> list)
    {
        foreach (TodoList item in list)
        {
            TodoListControl todoListControl = new TodoListControl(true)
            {
                DataContext = item,
                Height = 70,
                Margin = new Thickness(10, 7, 10, 7)
            };

            Binding IdBinding = new Binding
            {
                Path = new PropertyPath("Id"),
                Mode = BindingMode.OneWay
            };

            Binding NameBinding = new Binding
            {
                Path = new PropertyPath("Name"),
                Mode = BindingMode.TwoWay
            };

            Binding ColorBinding = new Binding
            {
                Path = new PropertyPath("Color"),
                Mode = BindingMode.TwoWay
            };

            todoListControl.PropertyChanged += TodoListControl_PropertyChanged;
            todoListControl.DeleteClicked += TodoListControl_DeleteClicked;

            todoListControl.SetBinding(TodoListControl.IdProperty, IdBinding);
            todoListControl.SetBinding(TodoListControl.NameProperty, NameBinding);
            todoListControl.SetBinding(TodoListControl.ColorProperty, ColorBinding);

            TodoListListView.Items.Add(todoListControl);
        }
    }

    private void TodoListControl_DeleteClicked(object? sender, EventArgs e)
    {
        TodoListControl deletedControl = sender as TodoListControl;
        if (deletedControl != null)
        {
            int index = TodoListListView.Items.IndexOf(deletedControl);
            int newIndex = Math.Max(index - 1, 0);
            TodoListControl newControl = TodoListListView.Items.OfType<TodoListControl>().ElementAtOrDefault(newIndex);
            if (newControl != null)
            {
                TodoListListView.SelectedItem = newControl;
            }
            else
            {
                TitleTextBlock.Text = "";
                AddTodoItemButton.Visibility = Visibility.Collapsed;
                TodoItemListView.Items.Clear();
            }
        }
    }

    private void TodoListControl_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Name")
        {
            TodoListControl control = sender as TodoListControl;
            if (control != null && control.IsSelected)
            {
                TitleTextBlock.Text = control.Name;
            }
        }
        
    }

    private void TodoLists_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            LoadTodoFromList(e.NewItems.Cast<TodoList>());
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        {
            foreach (TodoList item in e.OldItems)
            {
                TodoListControl todoListControl = TodoListListView.Items.OfType<TodoListControl>().FirstOrDefault(x => x.DataContext == item);
                TodoListListView.Items.Remove(todoListControl);
            }
        }
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        TodoList todoList = new TodoList
        {
            Id = 1,
            Name = "New List",
            Todos = new ObservableCollection<Todo>(),
            Color = Color.FromArgb(255, 255, 255, 255)
        };

        TodoCollection.TodoLists.Add(todoList);

        TodoListControl newControl = TodoListListView.Items.OfType<TodoListControl>().LastOrDefault();
        if (newControl != null)
        {
            TodoListListView.SelectedItem = newControl;
        }

        AddButton.IsEnabled = false;
        await Task.Delay(TimeSpan.FromSeconds(0.1));
        AddButton.IsEnabled = true;
    }

    private async void AddTodoItemButton_Click(object sender, RoutedEventArgs e)
    {
        if (TodoListListView.SelectedItem is TodoListControl selectedControl && selectedControl.DataContext is TodoList selectedTodoList)
        {
            AddTodo addTodo = new AddTodo(selectedTodoList);
            addTodo.XamlRoot = this.XamlRoot;
            await addTodo.ShowAsync();

            UpdateTodoListView(selectedTodoList);
        }
    }

    private void UpdateTodoListView(TodoList selected)
    {
        TodoItemListView.Items.Clear();
        foreach (Todo todo in selected.Todos)
        {
            TodoControl todoItemControl = new TodoControl()
            {
                DataContext = todo,
                Height = 70,
                Margin = new Thickness(40, 0, 40, 0)
            };

            Binding TitleBinding = new Binding
            {
                Path = new PropertyPath("Title"),
                Mode = BindingMode.TwoWay
            };

            Binding DescriptionBinding = new Binding
            {
                Path = new PropertyPath("Description"),
                Mode = BindingMode.TwoWay
            };

            Binding DueDateBinding = new Binding
            {
                Path = new PropertyPath("DueDate"),
                Mode = BindingMode.TwoWay
            };

            Binding IsCompletedBinding = new Binding
            {
                Path = new PropertyPath("IsCompleted"),
                Mode = BindingMode.TwoWay
            };

            Binding PriorityBinding = new Binding
            {
                Path = new PropertyPath("Priority"),
                Mode = BindingMode.TwoWay
            };

            Binding SubtasksBinding = new Binding
            {
                Path = new PropertyPath("Subtasks"),
                Mode = BindingMode.TwoWay
            };

            todoItemControl.SetBinding(TodoControl.TitleProperty, TitleBinding);
            todoItemControl.SetBinding(TodoControl.DescriptionProperty, DescriptionBinding);
            todoItemControl.SetBinding(TodoControl.DueDateProperty, DueDateBinding);
            todoItemControl.SetBinding(TodoControl.IsCompletedProperty, IsCompletedBinding);
            todoItemControl.SetBinding(TodoControl.PriorityProperty, PriorityBinding);
            todoItemControl.SetBinding(TodoControl.SubtasksProperty, SubtasksBinding);

            todoItemControl.DeleteTodo += TodoItemControl_DeleteTodo;

            TodoItemListView.Items.Add(todoItemControl);
        }
    }

    private void TodoItemControl_DeleteTodo(Todo todo)
    {
        if (TodoListListView.SelectedItem is TodoListControl selectedControl && selectedControl.DataContext is TodoList selectedTodoList)
        {
            selectedTodoList.Todos.Remove(todo);
            UpdateTodoListView(selectedTodoList);
        }
    }
}
