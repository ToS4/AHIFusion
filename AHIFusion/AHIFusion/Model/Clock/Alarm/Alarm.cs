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

    private Dictionary<string, bool> days = new Dictionary<string, bool>();
    public Dictionary<string, bool> Days
    {
        get
        {
            return days;
        }
        set
        {
            if (days != value)
            {
                days = value;
                onPropertyChanged(nameof(Days));
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

    private static string soundDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds");
    public static List<string?> SoundPaths { get; private set; } = Directory.EnumerateFiles(soundDirectory, "*.*", SearchOption.AllDirectories)
    .Where(file => file.EndsWith(".mp3") || file.EndsWith(".wav"))
    .Select(Path.GetFileName)
    .ToList();

    private string sound;
    public string Sound
    {
        get
        {
            return sound;
        }
        set
        {
            if (sound != value)
            {
                sound = value;
                onPropertyChanged(nameof(Sound));
            }
        }
    }

    public Alarm(string title, TimeOnly time, bool isOn, string sound)
    {
        Title = title;
        Time = time;
        IsOn = isOn;
        Sound = sound;
    }

    public Alarm() { }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void onPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
