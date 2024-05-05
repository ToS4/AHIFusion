using System.ComponentModel;

namespace AHIFusion.Model;
public class Note : INotifyPropertyChanged
{
    private string title;
    public string Title
    {
        get { return title; }
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    private string text;
    public string Text
    {
        get { return text; }
        set
        {
            if (text != value)
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    }

    private bool isSelected;
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            if (isSelected != value)
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    private Visibility visibility;
    public Visibility Visibility
    {
        get { return visibility; }
        set
        {
            if (visibility != value)
            {
                visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }
    }

    public Note(string title, string text)
    {
        Title = title;
        Text = text;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
