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
using Markdig;
using iText.Kernel.Pdf;
using iText.Html2pdf;
using Windows.ApplicationModel.Core;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AHIFusion
{
    public partial class NotesPage : Page
	{
        private ObservableCollection<SelectableNote> notesFiltered = new ObservableCollection<SelectableNote>();
        private bool navigationUsage = true;
        public NotesPage()
        {
            try
            {
                Log.Information("Initializing NotesPage");

                this.InitializeComponent();

                Log.Debug("Loading notes from NoteCollection");

                int loaded = 0;

                foreach (Note note in NoteCollection.Notes)
                {
                    loaded += 1;
                    notesFiltered.Add(new SelectableNote { Note = note, IsSelected = false });
                }

                Log.Debug("Loaded {LoadedCount} out of {TotalCount} notes", loaded, NoteCollection.Notes.Count);

                NotesListView.ItemsSource = notesFiltered;

                Log.Debug("Connecting XAML events");

                NoteCollection.Notes.CollectionChanged += Notes_CollectionChanged;

                EditorRichEditBox.IsTabStop = true;
                EditorRichEditBox.TabNavigation = KeyboardNavigationMode.Local;
                EditorRichEditBox.Paste += EditorRichEditBox_Paste;

                EditorWebView.NavigationStarting += EditorWebView_NavigationStarting;

                Log.Debug("Setup");

                Update();
                LoadText();
                SetupCustomCommandBarFlyout();

                this.DataContext = this;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        private void SetupCustomCommandBarFlyout()
        {
            try
            {
                Log.Information("Changing default CommandBarFlyout for RichtEditBox");

                Log.Debug("Create a new CommandBarFlyout");
                CommandBarFlyout myFlyout = new CommandBarFlyout();

                Log.Debug("Create new AppBarButtons");
                AppBarButton boldButton = new AppBarButton { Icon = new SymbolIcon(Symbol.Bold), Label = "Bold" };
                ToolTipService.SetToolTip(boldButton, "Make text bold");
                boldButton.Click += (s, ev) => { ApplyMarkdownSyntax("**"); };

                AppBarButton italicButton = new AppBarButton { Icon = new SymbolIcon(Symbol.Italic), Label = "Italic" };
                ToolTipService.SetToolTip(italicButton, "Make text italic");
                italicButton.Click += (s, ev) => { ApplyMarkdownSyntax("*"); };

                AppBarButton strikethroughButton = new AppBarButton
                {
                    Icon = new FontIcon { Glyph = "\uEDE0", FontFamily = new FontFamily("Segoe MDL2 Assets") },
                    Label = "Strikethrough"
                };
                ToolTipService.SetToolTip(strikethroughButton, "Strike through text");
                strikethroughButton.Click += (s, ev) => { ApplyMarkdownSyntax("~~"); };

                Log.Debug("Add the AppBarButtons to the CommandBarFlyout");
                myFlyout.PrimaryCommands.Add(boldButton);
                myFlyout.PrimaryCommands.Add(italicButton);
                myFlyout.PrimaryCommands.Add(strikethroughButton);

                Log.Debug("Assign the CommandBarFlyout to the RichEditBox's ContextFlyout property");
                EditorRichEditBox.ContextFlyout = myFlyout;
                EditorRichEditBox.SelectionFlyout = myFlyout;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        private void ApplyMarkdownSyntax(string syntax)
        {
            try
            {
                Log.Information($"Applying markdown syntax {syntax} to selected text");

                Log.Debug("Get the current selection");
                ITextSelection selection = EditorRichEditBox.Document.Selection;

                if (!string.IsNullOrEmpty(selection.Text))
                {
                    Log.Debug("Apply the markdown syntax to the selected text");
                    selection.Text = syntax + selection.Text + syntax;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
            }
        }

        private async void EditorWebView_NavigationStarting(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            try
            {
                Log.Information("EditorWebView navigation changing");

                if (!navigationUsage)
                {
                    Log.Debug("Cancel the navigation");
                    args.Cancel = true;
                }

                navigationUsage = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
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
                    Log.Debug("Pasting clipboard content");
                    var text = await dataPackageView.GetTextAsync();
                    ITextRange textRange = EditorRichEditBox.Document.Selection;
                    textRange.Text = text;
                    EditorRichEditBox.Document.Selection.StartPosition = EditorRichEditBox.Document.Selection.EndPosition;
                    Log.Debug("Pasted text of length {Length}", text.Length);
                }
                else
                {
                    Log.Debug("Clipboard content contains no text");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while pasting text");
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
                    Log.Debug("No text selected or no link selected");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
                
            }
        }

        private string InsertHyperlink(string rtf, string selectedText, string url)
        {
            try
            {
                Log.Information("Inserting hyperlink into RTF");

                Log.Debug("Find the position of the selected text in the RTF");
                int startIndex = rtf.IndexOf(selectedText);
                if (startIndex < 0)
                {
                    Log.Debug("Selected text not found in RTF");
                    return rtf;
                }

                Log.Debug("Create RTF for the hyperlink");
                string hyperlinkRtf = @"{\field{\*\fldinst HYPERLINK """ + url + @"""}{\fldrslt " + selectedText + @"}}";

                Log.Debug("Insert the hyperlink RTF at the position of the selected text");
                return rtf.Substring(0, startIndex) + hyperlinkRtf + rtf.Substring(startIndex + selectedText.Length);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
                return rtf;
                
            }
        }

        private void ApplyStyle()
        {
            try
            {
                Log.Information("Applying style to text");

                if (true)
                {
                    Log.Debug("ApplyStyle is disabled");
                    return;
                }

                int size = 16;
                string name = "Arial";

                var selectedItem = NotesListView.SelectedItem as SelectableNote;

                if (selectedItem != null)
                {
                    if (selectedItem.FontName == null)
                    {
                        selectedItem.FontName = name;
                    }

                    if (selectedItem.FontSize <= 0)
                    {
                        selectedItem.FontSize = size;
                    }

                    size = selectedItem.FontSize;
                    name = selectedItem.FontName;
                }

                Log.Debug($"size {size}, name {name}");

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
                Log.Error(ex, "An error occurred");
            }
        }

        private void LoadText()
        {
            try
            {
                Log.Information("Loading text");

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

                    Log.Debug("Setting text from selected note");
                    EditorRichEditBox.Document.SetText(TextSetOptions.FormatRtf, selectedItem.Note.Text);

                    Log.Debug("Removing all empty lines");
                    ITextRange textRange = EditorRichEditBox.Document.GetRange(0, TextConstants.MaxUnitCount);
                    while (textRange.Text.EndsWith("\r"))
                    {
                        textRange.Text = textRange.Text.Remove(textRange.Text.Length - 1);
                    }
                }

                ApplyStyle();

                UpdateMarkdownView();
            }

            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
                
            }
        }

        private void EditorRichEditBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadText();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
                
            }
        }

        private void EditorRichEditBox_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("EditorRichEditBox unloaded");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                return false;
                
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
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                return null;
                
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
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                
            }
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
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
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                
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
                Log.Error(ex, "An error occurred");
                
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
                fileSavePicker.FileTypeChoices.Add("Markdown", new List<string>() { ".md" });
                fileSavePicker.FileTypeChoices.Add("PDF", new List<string>() { ".pdf" });

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, hwnd);

                StorageFile saveFile = await fileSavePicker.PickSaveFileAsync();
                if (saveFile != null)
                {
                    Log.Information("A file was selected to save");

                    CachedFileManager.DeferUpdates(saveFile);

                    EditorRichEditBox.Document.GetText(TextGetOptions.None, out string currentText);
                    string htmlText = getHtmlString();

                    if (saveFile.FileType == ".pdf")
                    {

                        // Creating temp.html file
                        File.WriteAllText("temp.html", htmlText);

                        // Converting to pdf
                        HtmlConverter.ConvertToPdf(
                                new FileInfo("temp.html"),
                                new FileInfo(saveFile.Path)
                                );

                        // Deleting temp.html file
                        File.Delete("temp.html");
                    }
                    else // Markdown
                    {
                        await FileIO.WriteTextAsync(saveFile, currentText);
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
                Log.Error(ex, "An error occurred");
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
                Log.Error(ex, "An error occurred");
                
            }
        }

        private string getHtmlString()
        {
            EditorRichEditBox.Document.GetText(TextGetOptions.None, out string currentText);
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            string htmlText = Markdown.ToHtml(currentText, pipeline);
            return htmlText;
        }

        private async void UpdateMarkdownView()
        {
            Log.Information("Updating text changed event triggered");

            await EditorWebView.EnsureCoreWebView2Async();

            navigationUsage = true;
            EditorWebView.NavigateToString(getHtmlString());
        }

        private void EditorRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("EditorRichEditBox text changed event triggered");

                UpdateMarkdownView();

                SaveText();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred");
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
                Log.Error(ex, "An error occurred");
                
            }
        }
    }
}
