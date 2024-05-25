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
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using Microsoft.UI.Xaml.Documents;
using System;
using Microsoft.UI;
using Windows.ApplicationModel.DataTransfer;
using System.Text.RegularExpressions;
using ABI.System;

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

            EditorRichEditBox.IsTabStop = true;
            EditorRichEditBox.TabNavigation = KeyboardNavigationMode.Local;
            EditorRichEditBox.Paste += EditorRichEditBox_Paste;

            Update();
            ApplyStyle(16, "Arial");

            this.DataContext = this;
        }

        private async void EditorRichEditBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            e.Handled = true;

            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                var text = await dataPackageView.GetTextAsync();
                ITextRange textRange = EditorRichEditBox.Document.GetRange(TextConstants.MaxUnitCount, TextConstants.MaxUnitCount);
                textRange.Text = text;
            }
        }
        private void Menu_Opening(object sender, object e)
        {
            CommandBarFlyout myFlyout = sender as CommandBarFlyout;
            if (myFlyout.Target == EditorRichEditBox)
            {
                AppBarButton hyperlinkButton = new AppBarButton();
                hyperlinkButton.Icon = new SymbolIcon(Symbol.Link);
                //hyperlinkButton.Label = "Hyperlink";
                hyperlinkButton.Click += HyperlinkButton_Click;

                myFlyout.PrimaryCommands.Add(hyperlinkButton);
            }
        }
        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            List<SelectableNote> allNotes = new List<SelectableNote>();

            foreach (Note note in NoteCollection.Notes)
            {
                allNotes.Add(new SelectableNote { Note = note, IsSelected = false });
            }

            LinkText linkText = new LinkText(allNotes);
            linkText.XamlRoot = this.XamlRoot;

            await linkText.ShowAsync();

            string selectedText;
            EditorRichEditBox.Document.Selection.GetText(TextGetOptions.None, out selectedText);

            if (!string.IsNullOrEmpty(selectedText) && !string.IsNullOrEmpty(linkText.selectedLink))
            {
                // Save the RichEditBox document's current content
                string originalRtf;
                EditorRichEditBox.Document.GetText(TextGetOptions.FormatRtf, out originalRtf);

                // Format the selected text as a hyperlink
                var rtfWithLink = InsertHyperlink(originalRtf, selectedText, linkText.selectedLink);

                // Set the RichEditBox content to the updated RTF with the hyperlink
                EditorRichEditBox.Document.SetText(TextSetOptions.FormatRtf, rtfWithLink);
            }
        }

        private string InsertHyperlink(string rtf, string selectedText, string url)
        {
            // Find the position of the selected text in the RTF
            int startIndex = rtf.IndexOf(selectedText);
            if (startIndex < 0) return rtf; // Selected text not found in RTF

            // Create RTF for the hyperlink
            string hyperlinkRtf = @"{\field{\*\fldinst HYPERLINK """ + url + @"""}{\fldrslt " + selectedText + @"}}";

            // Insert the hyperlink RTF at the position of the selected text
            return rtf.Substring(0, startIndex) + hyperlinkRtf + rtf.Substring(startIndex + selectedText.Length);
        }

        private void ApplyStyle(int size, string name)
        {
            ITextCharacterFormat noteFormat = EditorRichEditBox.Document.GetDefaultCharacterFormat();
            noteFormat.Size = size;
            noteFormat.Name = name;

            EditorRichEditBox.Document.SetDefaultCharacterFormat(noteFormat);

            ITextRange textRange = EditorRichEditBox.Document.GetRange(TextConstants.MinUnitCount, TextConstants.MaxUnitCount);
            textRange.CharacterFormat.Size = size;
            textRange.CharacterFormat.Name = name;
            
            //EditorRichEditBox.Document.Selection.StartPosition = 0;
        }

        private void LoadText()
        {
            var selectedItem = NotesListView.SelectedItem as SelectableNote;

            if (selectedItem != null)
            {

                if (selectedItem.FontName == null)
                {
                    selectedItem.FontName = "Arial";
                }

                if (selectedItem.FontSize <= 0)
                {
                    selectedItem.FontSize = 16;
                }

                EditorRichEditBox.Document.SetText(TextSetOptions.FormatRtf, selectedItem.Note.Text);

                ApplyStyle(selectedItem.FontSize, selectedItem.FontName);
            }
            else
            {
                ApplyStyle(16, "Arial");
            };
        }

        private void EditorRichEditBox_Loaded(object sender, RoutedEventArgs e)
        {
            EditorRichEditBox.SelectionFlyout.Opening += Menu_Opening;
            EditorRichEditBox.ContextFlyout.Opening += Menu_Opening;
            LoadText();
        }

        private void EditorRichEditBox_Unloaded(object sender, RoutedEventArgs e)
        {
            EditorRichEditBox.SelectionFlyout.Opening -= Menu_Opening;
            EditorRichEditBox.ContextFlyout.Opening -= Menu_Opening;
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

                RightViewGrid.Visibility = Visibility.Visible;
                

                var selectedItem = e.AddedItems[0] as SelectableNote;
                selectedItem.IsSelected = true;

                Binding Titlebinding = new Binding();
                Titlebinding.Source = selectedItem.Note;
                Titlebinding.Path = new PropertyPath("Title");
                RightViewNoteTitleTextBlock.SetBinding(TextBlock.TextProperty, Titlebinding);

                if (e.RemovedItems.Count > 0)
                {
                    var oldSelectedItem = e.RemovedItems[0] as SelectableNote;
                    oldSelectedItem.IsSelected = false;
                }

                LoadText();
            }
            else
            {
                RightViewGrid.Visibility = Visibility.Collapsed;
                RightViewNoteTitleTextBlock.Text = "";
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

        private void SaveText()
        {
            var selectedItem = NotesListView.SelectedItem as SelectableNote;

            if (selectedItem != null)
            {
                EditorRichEditBox.Document.GetText(TextGetOptions.FormatRtf, out string formatedText);
                selectedItem.Note.Text = formatedText;
            }
        }

        private void EditorRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            SaveText();
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedColor = (Button)sender;
            var rectangle = (Microsoft.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((Microsoft.UI.Xaml.Media.SolidColorBrush)rectangle.Fill).Color;

            EditorRichEditBox.Document.Selection.CharacterFormat.ForegroundColor = color;

            SaveText();
            LoadText();

            fontColorButton.Flyout.Hide();
            EditorRichEditBox.Focus(Microsoft.UI.Xaml.FocusState.Keyboard);
        }
    }
}
