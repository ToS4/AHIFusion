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
using Windows.Storage.Streams;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Controls;

namespace AHIFusion
{
    public partial class NotesPage : Page
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

            this.DataContext = this;
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

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".rtf");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(open, hwnd);

            StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream randAccStream =
                    await file.OpenAsync(FileAccessMode.Read))
                {
                    EditorRichEditBox.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);
                }
            }
        }

        private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            var fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            fileSavePicker.SuggestedFileName = "New Document";
            fileSavePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, hwnd);

            StorageFile saveFile = await fileSavePicker.PickSaveFileAsync();
            if (saveFile != null)
            {
                CachedFileManager.DeferUpdates(saveFile);

                using (IRandomAccessStream randAccStream =
                    await saveFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    EditorRichEditBox.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
                }

                //await CachedFileManager.CompleteUpdatesAsync(saveFile);
            }
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            EditorRichEditBox.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            EditorRichEditBox.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            EditorRichEditBox.Document.Selection.CharacterFormat.Underline = UnderlineType.Single;
        }
    }
}
