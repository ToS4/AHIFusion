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
        this.InitializeComponent();
        todoEdit = todo;

        AddedSubtasks.CollectionChanged += AddedSubtasks_CollectionChanged;

        TitleEdit = todo.Title;
        DescriptionEdit = todo.Description;
        DueDateEdit = todo.DueDate;
        IsCompletedEdit = todo.IsCompleted;
        PriorityEdit = todo.Priority;
        SubtasksEdit = todo.Subtasks;

        SetPriorityRadioButton();

        AddSubTodo(SubtasksEdit);
    }

    private void AddedSubtasks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            AddSubTodo(e.NewItems.Cast<TodoSub>());
        }
    }

    private void AddSubTodo(IEnumerable<TodoSub> list)
    {
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
        }
    }

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
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

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
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
    private void SetPriorityRadioButton()
    {
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


    private void SubTaskButton_Click(object sender, RoutedEventArgs e)
    {
        TodoSub todoSub = new TodoSub()
        {
            Id = Guid.NewGuid(),
            Title = "New Subtask",
            IsCompleted = false
        };
        AddedSubtasks.Add(todoSub);
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        DeleteTodo?.Invoke(todoEdit);
        this.Hide();
    }

    private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        for (int i = 0; i < AddedSubtasks.Count; i++)
        {
            SubtasksEdit.Remove(AddedSubtasks[i]);
        }
    }
}
