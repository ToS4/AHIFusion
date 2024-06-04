using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHIFusion;
public class Stopwatch : INotifyPropertyChanged
{
    private DateTime startTime;
    public DateTime StartTime
    {
        get
        {
            return startTime;
        }
        set
        {
            startTime = value;
            OnPropertyChanged("StartTime");
        }
    }

    private TimeSpan elapsedTime;
    public TimeSpan ElapsedTime
    {
        get
        {
            return elapsedTime;
        }
        set
        {
            elapsedTime = value;
            OnPropertyChanged("ElapsedTime");
        }
    }

    private bool isRunning;
    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        set
        {
            isRunning = value;
            OnPropertyChanged("IsRunning");
        }
    }

    public Stopwatch() { }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
