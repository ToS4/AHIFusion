using System.Collections.ObjectModel;
using AHIFusion.Model;
using System.Linq;
using Windows.ApplicationModel.VoiceCommands;
using Uno.Extensions.Specialized;
using System.ComponentModel;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Microsoft.UI.Text;
using Windows.UI;

namespace AHIFusion
{
    public sealed partial class NotesPage : Page
	{
        private ObservableCollection<SelectableNote> notesFiltered = new ObservableCollection<SelectableNote>();

        public NotesPage()
        {
            this.InitializeComponent();

            foreach (Note note in NoteCollection.Notes)
            {
                notesFiltered.Add(new SelectableNote { Note = note, IsSelected = false});
            }

            NotesListView.ItemsSource = notesFiltered;

            NoteCollection.Notes.CollectionChanged += Notes_CollectionChanged;

            Update();
        }

        private void Notes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
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
                if (!filteredData.Contains(item.Note))
                {
                    notesFiltered.Remove(item);
                }
            }
        }
        
        private SelectableNote GetSelectableNote(Note note)
        {
            foreach (var item in notesFiltered)
            {
                if (item.Note == note)
                {
                    return item;
                }
            }

            SelectableNote newItem = new SelectableNote()
            {
                Note = note,
                IsSelected = false
            };

            return newItem;
        }

        private void AddMatching(IEnumerable<Note> filteredData)
        {
            foreach (var item in filteredData)
            {
                SelectableNote note = GetSelectableNote(item);

                if (note != null)
                {
                    if (!notesFiltered.Contains(note))
                    {
                        notesFiltered.Add(note);
                    }
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            SelectableNote selectableNote = new SelectableNote()
            {
                Note = new Note("Untitled", ""),
                IsSelected = false
            };

            NoteCollection.Add(selectableNote.Note);;
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = NotesListView.SelectedItem as SelectableNote;

            if (selectedItem != null)
            {
                NoteCollection.Remove(selectedItem.Note);
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
                var selectedItem = e.AddedItems[0] as SelectableNote;
                selectedItem.IsSelected = true;

                if (e.RemovedItems.Count > 0)
                {
                    var oldSelectedItem = e.RemovedItems[0] as SelectableNote;
                    oldSelectedItem.IsSelected = false;
                }
            }
        }
    }
}
