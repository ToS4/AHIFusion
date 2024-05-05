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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
