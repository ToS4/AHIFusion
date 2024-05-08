using System.Collections.ObjectModel;
using Windows.ApplicationModel.VoiceCommands;

namespace AHIFusion.Model;
public static class NoteCollection
{
    public static ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

    public static void Add(Note note)
    {
        Notes.Add(note);
    }
    public static void Remove(Note note)
    {
        Notes.Remove(note);
    }
}
