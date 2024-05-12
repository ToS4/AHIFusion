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
                onPropertyChanged(nameof(Title));
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
                onPropertyChanged(nameof(Time));
            }
        }
    }

    private bool isOn;
    public bool IsOn
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
                onPropertyChanged(nameof(IsOn));
            }
        }
    }

    public Alarm(string title, TimeOnly time, bool isOn)
    {
        Title = title;
        Time = time;
        IsOn = isOn;
    }

    public Alarm() { }


    public event PropertyChangedEventHandler PropertyChanged;

    private void onPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
