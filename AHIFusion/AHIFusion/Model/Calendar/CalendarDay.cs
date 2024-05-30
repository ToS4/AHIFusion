using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHIFusion;
public class CalendarDay : INotifyPropertyChanged
{
    public ObservableCollection<DayEvent> Events { get; set; } = new ObservableCollection<DayEvent>();

    private DateOnly date;
    public DateOnly Date
    {
        get { return date; }
        set
        {
            if (date != value)
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
