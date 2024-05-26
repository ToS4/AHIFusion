
using System.ComponentModel;

namespace AHIFusion.Model;

public class Timer : INotifyPropertyChanged
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

    private TimeSpan time;
    public TimeSpan Time
    {
        get { return time; }
        set
        {
            if (time != value)
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
    }

    private bool isRunning;
    public bool IsRunning
    {
        get { return isRunning; }
        set
        {
            if (isRunning != value)
            {
                isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }
    }

    private string sound;
    public string Sound
    {
        get { return sound; }
        set
        {
            if (sound != value)
            {
                sound = value;
                OnPropertyChanged(nameof(Sound));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
