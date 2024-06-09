using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Serilog;

namespace AHIFusion
{
	public sealed partial class AddTodo : ContentDialog
	{
        public string TitleAdd { get; set; } = "New Todo";
        public string DescriptionAdd { get; set; } = "";
        public DateTimeOffset DueDateAdd { get; set; } = DateTime.Now;
        public bool IsCompletedAdd { get; set; } = false;
        public int PriorityAdd { get; set; } = 1;
        public ObservableCollection<TodoSub> SubtasksAdd { get; set; } = new ObservableCollection<TodoSub>();

        public TodoList todoList { get; set; }
      
		public AddTodo(TodoList tl)
		{
            try
            {
                Log.Information("Initializing AddTodo");
                this.InitializeComponent();

                todoList = tl;

                Log.Debug("Connecting XAML events");
                SubtasksAdd.CollectionChanged += SubtasksAdd_CollectionChanged;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
		}

        private void SubtasksAdd_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                Log.Information("SubtasksAdd collection has been changed");

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
                Log.Information("ContentDialogPrimaryButton has been clicke");

                if (TitleAdd == "" || (PriorityAdd != 1 && PriorityAdd != 2 && PriorityAdd != 3))
                {
                    args.Cancel = true;
                    return;
                }

                DueDatePicker.Date = DueDateAdd;

                Todo todoToAdd = new Todo()
                {
                    Id = Guid.NewGuid(),
                    Title = TitleAdd,
                    Description = DescriptionAdd,
                    DueDate = DueDateAdd.DateTime,
                    IsCompleted = IsCompletedAdd,
                    Priority = PriorityAdd,
                    Subtasks = SubtasksAdd
                };

                todoList.Todos.Add(todoToAdd);
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
                            PriorityAdd = 1;
                            break;
                        case "Medium":
                            PriorityAdd = 2;
                            break;
                        case "High":
                            PriorityAdd = 3;
                            break;
                    }
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
                SubtasksAdd.Add(new TodoSub());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
            
        }
    }
}
