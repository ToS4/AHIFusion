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

namespace AHIFusion
{
	public sealed partial class AddTodo : ContentDialog
	{
        public string TitleAdd { get; set; } = "New Todo";
        public string DescriptionAdd { get; set; } = "";
        public DateTimeOffset DueDateAdd { get; set; } = DateTime.Now;
        public bool IsCompletedAdd { get; set; } = false;
        public int PriorityAdd { get; set; } = 0;
        public ObservableCollection<TodoSub> SubtasksAdd { get; set; } = new ObservableCollection<TodoSub>();

        public TodoList todoList { get; set; }
      
		public AddTodo(TodoList tl)
		{
			this.InitializeComponent();
            todoList = tl;

		}

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
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
    }
}
