using System.ComponentModel;

namespace AHIFusion.Model;
public class Alarm : INotifyPropertyChanged
{
    private string title;
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            if (title != value)
            {
                title = value;
                onPropertyChanged("Title");
            }
        }
    }

    private TimeOnly time;
    public TimeOnly Time
    {
        get
        {
            return time;
        }
        set
        {
            if (time != value)
            {
                time = value;
                onPropertyChanged("Time");
            }
        }
    }

    private bool isOn;
    private bool IsOn
    {
        get
        {
            return isOn;
        }
        set
        {
            if (isOn != value)
            {
                isOn = value;
                onPropertyChanged("IsOn");
            }
        }
    }

    public Alarm(string title, TimeOnly time)
    {
        Title = title;
        Time = time;
    }

    public Alarm() { }


    public event PropertyChangedEventHandler PropertyChanged;

    private void onPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
