using System.Collections.ObjectModel;
using AHIFusion.Model;
using System.Linq;

namespace AHIFusion
{
    public sealed partial class NotesPage : Page
	{
        ObservableCollection<Note> notesFiltered = new ObservableCollection<Note>();

        public NotesPage()
        {
            this.InitializeComponent();

            notesFiltered = new ObservableCollection<Note>(NoteCollection.Notes);
            notesListView.ItemsSource = notesFiltered;
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

        private bool Filter(Note note)
        {
            return note.Title.Contains(SearchTextBox.Text, StringComparison.InvariantCultureIgnoreCase);
        }

        private void Remove_NonMatching(IEnumerable<Note> filteredData)
        {
            for (int i = notesFiltered.Count - 1; i >= 0; i--)
            {
                var item = notesFiltered[i];
                // If contact is not in the filtered argument list, remove it from the ListView's source.
                if (!filteredData.Contains(item))
                {
                    notesFiltered.Remove(item);
                }
            }
        }

        private void AddBack_Notes(IEnumerable<Note> filteredData)
        {
            foreach (var item in filteredData)
            {
                // If item in filtered list is not currently in ListView's source collection, add it back in
                if (!notesFiltered.Contains(item))
                {
                    notesFiltered.Add(item);
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = NoteCollection.Notes.Where(note => Filter(note));
            Remove_NonMatching(filtered);
            AddBack_Notes(filtered);
        }
    }
}
