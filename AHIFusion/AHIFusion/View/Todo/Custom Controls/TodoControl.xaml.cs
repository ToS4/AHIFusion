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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AHIFusion
{
	public sealed partial class TodoControl : UserControl
	{
        public event Action<Todo> DeleteTodo;
        public string DueDateStirng;

		public TodoControl()
		{
			this.InitializeComponent();
            BackgroundRect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 181, 126, 220));
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TodoControl), new PropertyMetadata(""));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(TodoControl), new PropertyMetadata(""));
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DueDateProperty = DependencyProperty.Register("DueDate", typeof(DateTime), typeof(TodoControl), new PropertyMetadata(DateTime.Now));
        public DateTime DueDate
        {
            get { return (DateTime)GetValue(DueDateProperty); }
            set
            {
                SetValue(DueDateProperty, value);
                DueDateStirng = value.ToString("MM/dd/yyyy");
            }
        }

        public static readonly DependencyProperty IsCompletedProperty = DependencyProperty.Register("IsCompleted", typeof(bool), typeof(TodoControl), new PropertyMetadata(false));
        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            set { SetValue(IsCompletedProperty, value); }
        }

        public static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(int), typeof(TodoControl), new PropertyMetadata(0));
        public int Priority
        {
            get { return (int)GetValue(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }

        public static readonly DependencyProperty SubtasksProperty = DependencyProperty.Register("Subtasks", typeof(ObservableCollection<TodoSub>), typeof(TodoControl), new PropertyMetadata(new ObservableCollection<TodoSub>()));
        public ObservableCollection<TodoSub> Subtasks
        {
            get { return (ObservableCollection<TodoSub>)GetValue(SubtasksProperty); }
            set { SetValue(SubtasksProperty, value); }
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            BackgroundRect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 150, 110, 200));
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            BackgroundRect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 181, 126, 220));
        }

        private async void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (this.DataContext is Todo todo)
            {
                EditTodo editTodoDialog = new EditTodo(todo)
                {
                    XamlRoot = this.XamlRoot
                };
                editTodoDialog.DeleteTodo += Todo_Delete;
                await editTodoDialog.ShowAsync();
            }
        }

        private void Todo_Delete(Todo todo)
        {
            DeleteTodo?.Invoke(todo);
        }
    }
}
