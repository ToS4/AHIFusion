using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using AHIFusion.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Serilog;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AHIFusion;
public sealed partial class EditTodo : ContentDialog
{
    public string TitleEdit { get; set; }
    public string DescriptionEdit { get; set; }
    public DateTimeOffset DueDateEdit { get; set; } = new DateTimeOffset();
    public bool IsCompletedEdit { get; set; }
    public int PriorityEdit { get; set; }
    public ObservableCollection<TodoSub> SubtasksEdit { get; set; } = new ObservableCollection<TodoSub>();
    public ObservableCollection<TodoSub> AddedSubtasks { get; set; } = new ObservableCollection<TodoSub>();

    public Todo todoEdit;

    public event Action<Todo> DeleteTodo;

    public EditTodo(Todo todo)
    {
        try
        {
            Log.Information("Initializing EditTodo");

            this.InitializeComponent();

            todoEdit = todo;
            TitleEdit = todo.Title;
            DescriptionEdit = todo.Description;
            DueDateEdit = todo.DueDate;
            IsCompletedEdit = todo.IsCompleted;
            PriorityEdit = todo.Priority;
            SubtasksEdit = todo.Subtasks;

            Log.Debug("Connecting XAML events");
            AddedSubtasks.CollectionChanged += AddedSubtasks_CollectionChanged;

            SetPriorityRadioButton();
            AddSubTodo(SubtasksEdit);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
        
    }

    private void AddedSubtasks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        try
        {
            Log.Information("AddedSubtasks collection changed");

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                AddSubTodo(e.NewItems.Cast<TodoSub>());
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
       
    }

    private void AddSubTodo(IEnumerable<TodoSub> list)
    {
        try
        {
            Log.Information("Adding subtodo(s)");

            Log.Debug("Going through the given IEnumerable");
            int amountAdded = 0;
            foreach (TodoSub todoSub in list)
            {
                TodoSubControl todoSubControl = new TodoSubControl()
                {
                    DataContext = todoSub,
                    Height = 40
                };

                Binding TitleBinding = new Binding
                {
                    Path = new PropertyPath("Title"),
                    Mode = BindingMode.TwoWay
                };

                Binding IsCompletedBinding = new Binding
                {
                    Path = new PropertyPath("IsCompleted"),
                    Mode = BindingMode.TwoWay
                };

                todoSubControl.SetBinding(TodoSubControl.TitleProperty, TitleBinding);
                todoSubControl.SetBinding(TodoSubControl.IsCompletedProperty, IsCompletedBinding);

                SubTaskListView.Items.Add(todoSubControl);
                amountAdded += 1;
            }

            Log.Debug($"{amountAdded} from {list.Count()} subtodo(s) has been added");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }

    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        try
        {
            Log.Information("ContentDialogPrimaryButton has been clicked");

            todoEdit.Title = TitleEdit;
            todoEdit.Description = DescriptionEdit;
            todoEdit.DueDate = DueDateEdit.DateTime;
            todoEdit.IsCompleted = IsCompletedEdit;
            todoEdit.Priority = PriorityEdit;

            for (int i = 0; i < AddedSubtasks.Count; i++)
            {
                SubtasksEdit.Add(AddedSubtasks[i]);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
       
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("RadioButton has been checked");

            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                switch (rb.Content)
                {
                    case "Low":
                        PriorityEdit = 1;
                        break;
                    case "Medium":
                        PriorityEdit = 2;
                        break;
                    case "High":
                        PriorityEdit = 3;
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }
    private void SetPriorityRadioButton()
    {
        try
        {
            Log.Information("SetPriorityRadioButton has been called");

            switch (PriorityEdit)
            {
                case 1:
                    LowPriorityRadioButton.IsChecked = true;
                    break;
                case 2:
                    MediumPriorityRadioButton.IsChecked = true;
                    break;
                case 3:
                    HighPriorityRadioButton.IsChecked = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }


    private void SubTaskButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("SubTaskButton has been clicked");

            TodoSub todoSub = new TodoSub()
            {
                Id = Guid.NewGuid(),
                Title = "New Subtask",
                IsCompleted = false
            };
            AddedSubtasks.Add(todoSub);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log.Information("DeleteButton has been clicked");

            DeleteTodo?.Invoke(todoEdit);
            this.Hide();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }

    private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        try
        {
            Log.Information("ContentDialogCloseButton has been clicked");

            for (int i = 0; i < AddedSubtasks.Count; i++)
            {
                SubtasksEdit.Remove(AddedSubtasks[i]);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }
    }
}
