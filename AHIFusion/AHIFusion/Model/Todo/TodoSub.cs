using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHIFusion;
public class TodoSub : INotifyPropertyChanged
{
    private Guid id;
    private string title;
    private bool isCompleted;

    public Guid Id
    {
        get { return id; }
        set
        {
            if (value != id)
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
    }

    public string Title
    {
        get { return title; }
        set
        {
            if (value != title)
            {
                title = value; 
                OnPropertyChanged("Title");
            }

        }
    }

    public bool IsCompleted
    {
        get { return isCompleted; }
        set
        {
            if (isCompleted != value)
            {
                isCompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
