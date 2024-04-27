using AHIFusion.Notes;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace AHIFusion
{
	public sealed partial class NotesPage : Page
	{
        public NotesPage()
        {
            this.InitializeComponent();

            Binding notesItemsSourceBinding = new Binding()
            {
                Source = NoteCollection.Notes,
                Mode = BindingMode.OneWay
            };
            notesListView.SetBinding(ListView.ItemsSourceProperty, notesItemsSourceBinding);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Note note = new Note("Untitled", "");
            NoteCollection.Add(note);
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = notesListView.SelectedItem as Note;

            if (selectedItem != null)
            {
                NoteCollection.Remove(selectedItem);
            }
        }

        private void notesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = e.AddedItems[0] as Note;
                selectedItem.IsSelected = true;

                if (e.RemovedItems.Count > 0)
                {
                    var oldSelectedItem = e.RemovedItems[0] as Note;
                    oldSelectedItem.IsSelected = false;
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            foreach (Note note in notesListView.Items)
            {
                if (note.Title.ToLower().Contains(searchText))
                {
                    note.Visibility = Visibility.Visible;
                }
                else
                {
                    note.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
