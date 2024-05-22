using System.ComponentModel;
using AHIFusion.Model;

namespace AHIFusion;
public class SelectableNote : INotifyPropertyChanged
{
    public Note Note { get; set; }

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

    private string fontName;
    public string FontName
    {
        get { return fontName; }
        set
        {
            if (fontName != value)
            {
                fontName = value;
                OnPropertyChanged(nameof(FontName));
            }
        }
    }

    private int fontSize;
    public int FontSize
    {
        get { return fontSize; }
        set
        {
            if (fontSize != value)
            {
                fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
