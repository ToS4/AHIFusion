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
using Serilog;

namespace AHIFusion
{
    public partial class NotesPage : Page
	{
        private ObservableCollection<SelectableNote> notesFiltered = new ObservableCollection<SelectableNote>();
        public NotesPage()
        {
            try
            {
                Log.Information("Initializing NotesPage");

                this.InitializeComponent();

                Log.Debug("Loading notes from NoteCollection");

                foreach (Note note in NoteCollection.Notes)
                {
                    notesFiltered.Add(new SelectableNote { Note = note, IsSelected = false });
                }

                NotesListView.ItemsSource = notesFiltered;

                Log.Debug("Connecting XAML events");

                NoteCollection.Notes.CollectionChanged += Notes_CollectionChanged;

                EditorRichEditBox.IsTabStop = true;
                EditorRichEditBox.TabNavigation = KeyboardNavigationMode.Local;
                EditorRichEditBox.Paste += EditorRichEditBox_Paste;

                Log.Debug("Updating Filter");

                Update();

                Log.Debug("Applying default text style");

                ApplyStyle(16, "Arial");

                this.DataContext = this;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while initializing NotesPage");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private async void EditorRichEditBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            try
            {
                Log.Information("Paste text event triggered");
                e.Handled = true;

                Log.Debug("Getting clipboard content");
                var dataPackageView = Clipboard.GetContent();

                if (dataPackageView.Contains(StandardDataFormats.Text))
                {
                    var text = await dataPackageView.GetTextAsync();
                    ITextRange textRange = EditorRichEditBox.Document.GetRange(TextConstants.MaxUnitCount, TextConstants.MaxUnitCount);
                    textRange.Text = text;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling paste event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void Menu_Opening(object sender, object e)
        {
            try
            {
                Log.Information("Menu opening event triggered");

                CommandBarFlyout myFlyout = sender as CommandBarFlyout;
                if (myFlyout.Target == EditorRichEditBox)
                {
                    AppBarButton hyperlinkButton = new AppBarButton();
                    hyperlinkButton.Icon = new SymbolIcon(Symbol.Link);
                    hyperlinkButton.Click += HyperlinkButton_Click;

                    myFlyout.PrimaryCommands.Add(hyperlinkButton);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while opening menu");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("Hyperlink button click event triggered");

                List<SelectableNote> allNotes = new List<SelectableNote>();

                Log.Debug("Adding notes to allNotes list");
                foreach (Note note in NoteCollection.Notes)
                {
                    allNotes.Add(new SelectableNote { Note = note, IsSelected = false });
                }

                Log.Debug("Creating LinkText object");
                LinkText linkText = new LinkText(allNotes);
                linkText.XamlRoot = this.XamlRoot;

                Log.Debug("Showing LinkText dialog");
                await linkText.ShowAsync();

                string selectedText;
                EditorRichEditBox.Document.Selection.GetText(TextGetOptions.None, out selectedText);

                if (!string.IsNullOrEmpty(selectedText) && !string.IsNullOrEmpty(linkText.selectedLink))
                {
                    Log.Debug("Selected text: {selectedText}, selected link: {selectedLink}", selectedText, linkText.selectedLink);

                    // Save the RichEditBox document's current content
                    string originalRtf;
                    EditorRichEditBox.Document.GetText(TextGetOptions.FormatRtf, out originalRtf);

                    // Format the selected text as a hyperlink
                    Log.Debug("Inserting hyperlink into selected text");
                    var rtfWithLink = InsertHyperlink(originalRtf, selectedText, linkText.selectedLink);

                    // Set the RichEditBox content to the updated RTF with the hyperlink
                    Log.Debug("Setting RichEditBox content to the updated RTF with the hyperlink");
                    EditorRichEditBox.Document.SetText(TextSetOptions.FormatRtf, rtfWithLink);
                }
                else
                {
                    Log.Warning("No text selected or no link selected");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling hyperlink button click event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private string InsertHyperlink(string rtf, string selectedText, string url)
        {
            try
            {
                Log.Debug("Inserting hyperlink into RTF");

                // Find the position of the selected text in the RTF
                int startIndex = rtf.IndexOf(selectedText);
                if (startIndex < 0)
                {
                    Log.Warning("Selected text not found in RTF");
                    return rtf; // Selected text not found in RTF
                }

                // Create RTF for the hyperlink
                string hyperlinkRtf = @"{\field{\*\fldinst HYPERLINK """ + url + @"""}{\fldrslt " + selectedText + @"}}";

                // Insert the hyperlink RTF at the position of the selected text
                return rtf.Substring(0, startIndex) + hyperlinkRtf + rtf.Substring(startIndex + selectedText.Length);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while inserting hyperlink into RTF");
                //return rtf;
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void ApplyStyle(int size, string name)
        {
            try
            {
                Log.Debug("Applying style: size {size}, name {name}", size, name);

                ITextCharacterFormat noteFormat = EditorRichEditBox.Document.GetDefaultCharacterFormat();
                noteFormat.Size = size;
                noteFormat.Name = name;

                EditorRichEditBox.Document.SetDefaultCharacterFormat(noteFormat);

                ITextRange textRange = EditorRichEditBox.Document.GetRange(TextConstants.MinUnitCount, TextConstants.MaxUnitCount);
                textRange.CharacterFormat.Size = size;
                textRange.CharacterFormat.Name = name;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while applying style");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void LoadText()
        {
            try
            {
                Log.Debug("Loading text");

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
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while loading text");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void EditorRichEditBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("EditorRichEditBox loaded");

                EditorRichEditBox.SelectionFlyout.Opening += Menu_Opening;
                EditorRichEditBox.ContextFlyout.Opening += Menu_Opening;
                LoadText();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while loading EditorRichEditBox");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void EditorRichEditBox_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("EditorRichEditBox unloaded");

                EditorRichEditBox.SelectionFlyout.Opening -= Menu_Opening;
                EditorRichEditBox.ContextFlyout.Opening -= Menu_Opening;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while unloading EditorRichEditBox");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void Notes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                Log.Information("Notes collection changed");

                Update();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling notes collection change");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void Update()
        {
            try
            {
                Log.Debug("Updating notes");

                var filtered = NoteCollection.Notes.Where(note => Filter(note));
                RemoveNonMatching(filtered);
                AddMatching(filtered);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating notes");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private bool Filter(Note note)
        {
            try
            {
                Log.Debug("Filtering notes");

                return note.Title.Contains(SearchTextBox.Text, StringComparison.InvariantCultureIgnoreCase);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while filtering notes");
                //return false;
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        private void RemoveNonMatching(IEnumerable<Note> filteredData)
        {
            try
            {
                Log.Debug("Removing non-matching notes");

                for (int i = notesFiltered.Count - 1; i >= 0; i--)
                {
                    var item = notesFiltered[i];
                    if (!filteredData.Contains(item.Note))
                    {
                        notesFiltered.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while removing non-matching notes");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        private SelectableNote GetSelectableNote(Note note)
        {
            try
            {
                Log.Debug("Getting selectable note");

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
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting selectable note");
                //return null;
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void AddMatching(IEnumerable<Note> filteredData)
        {
            try
            {
                Log.Debug("Adding matching notes");

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
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding matching notes");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("AddButton click event triggered");

                SelectableNote selectableNote = new SelectableNote()
                {
                    Note = new Note("Untitled", ""),
                    IsSelected = false
                };

                NoteCollection.Add(selectableNote.Note);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling AddButton click event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("DeleteButton click event triggered");

                var selectedItem = NotesListView.SelectedItem as SelectableNote;

                if (selectedItem != null)
                {
                    NoteCollection.Remove(selectedItem.Note);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling DeleteButton click event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Log.Information("SearchTextBox text changed event triggered");

                Update();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling SearchTextBox text changed event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void NotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Log.Information("NotesListView selection changed event triggered");

                if (e.AddedItems.Count > 0)
                {
                    Log.Information("An item was selected in NotesListView");

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
                    Log.Information("No items are selected in NotesListView");

                    RightViewGrid.Visibility = Visibility.Collapsed;
                    RightViewNoteTitleTextBlock.Text = "";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling NotesListView selection changed event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("OpenFileButton_Click event triggered");

                FileOpenPicker open = new FileOpenPicker();
                open.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                open.FileTypeFilter.Add(".rtf");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(open, hwnd);

                StorageFile file = await open.PickSingleFileAsync();

                if (file != null)
                {
                    Log.Information("A file was selected to open");

                    using (IRandomAccessStream randAccStream =
                        await file.OpenAsync(FileAccessMode.Read))
                    {
                        EditorRichEditBox.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);
                    }
                }
                else
                {
                    Log.Information("No file was selected to open");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling OpenFileButton click event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("SaveFileButton click event triggered");

                var fileSavePicker = new FileSavePicker();
                fileSavePicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
                fileSavePicker.SuggestedFileName = "New Document";
                fileSavePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, hwnd);

                StorageFile saveFile = await fileSavePicker.PickSaveFileAsync();
                if (saveFile != null)
                {
                    Log.Information("A file was selected to save");

                    CachedFileManager.DeferUpdates(saveFile);

                    using (IRandomAccessStream randAccStream =
                        await saveFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        EditorRichEditBox.Document.SaveToStream(TextGetOptions.FormatRtf, randAccStream);
                    }

                    //await CachedFileManager.CompleteUpdatesAsync(saveFile);
                }
                else
                {
                    Log.Information("No file was selected to save");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling SaveFileButton click event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void SaveText()
        {
            try
            {
                Log.Information("Saving text");

                var selectedItem = NotesListView.SelectedItem as SelectableNote;

                if (selectedItem != null)
                {
                    EditorRichEditBox.Document.GetText(TextGetOptions.FormatRtf, out string formatedText);
                    selectedItem.Note.Text = formatedText;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while saving text");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void EditorRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("EditorRichEditBox text changed event triggered");

                SaveText();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling EditorRichEditBox text changed event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("ColorButton click event triggered");

                Button clickedColor = (Button)sender;
                var rectangle = (Microsoft.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
                var color = ((Microsoft.UI.Xaml.Media.SolidColorBrush)rectangle.Fill).Color;

                EditorRichEditBox.Document.Selection.CharacterFormat.ForegroundColor = color;

                SaveText();
                LoadText();

                fontColorButton.Flyout.Hide();
                EditorRichEditBox.Focus(Microsoft.UI.Xaml.FocusState.Keyboard);

                Log.Information("Text color changed and text reloaded");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling ColorButton click event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
