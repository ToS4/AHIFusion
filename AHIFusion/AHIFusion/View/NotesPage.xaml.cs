using System.Collections.ObjectModel;
using AHIFusion.Model;
using System.Linq;
using Windows.ApplicationModel.VoiceCommands;

namespace AHIFusion
{
    public sealed partial class NotesPage : Page
	{
        private ObservableCollection<Note> notesFiltered = new ObservableCollection<Note>();

        public NotesPage()
        {
            this.InitializeComponent();

            notesFiltered = new ObservableCollection<Note>(NoteCollection.Notes);
            NotesListView.ItemsSource = notesFiltered;
            Update();
        }
        private void Update()
        {
            var filtered = NoteCollection.Notes.Where(note => Filter(note));
            RemoveNonMatching(filtered);
            AddMatching(filtered);
        }

        private bool Filter(Note note)
        {
            return note.Title.Contains(SearchTextBox.Text, StringComparison.InvariantCultureIgnoreCase);
        }
        
        private void RemoveNonMatching(IEnumerable<Note> filteredData)
        {
            for (int i = notesFiltered.Count - 1; i >= 0; i--)
            {
                var item = notesFiltered[i];
                if (!filteredData.Contains(item))
                {
                    notesFiltered.Remove(item);
                }
            }
        }
        
        private void AddMatching(IEnumerable<Note> filteredData)
        {
            foreach (var item in filteredData)
            {
                if (!notesFiltered.Contains(item))
                {
                    notesFiltered.Add(item);
                }
            }
        }
       
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Note note = new Note("Untitled", "");
            NoteCollection.Add(note);
            Update();
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = NotesListView.SelectedItem as Note;

            if (selectedItem != null)
            {
                NoteCollection.Remove(selectedItem);
                Update();
            }
        }
        
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }
        
        private void NotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
    }
}
