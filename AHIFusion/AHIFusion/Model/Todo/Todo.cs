using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHIFusion;
public class Todo : INotifyPropertyChanged
{
    private int id;
    private string title;
    private string description;
    private DateTime dueDate;
    private bool isCompleted;
    private int priority;
    private ObservableCollection<TodoSub> subtasks;

    public int Id
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

    public string Description
    {
        get { return description; }
        set
        {
            if (value != description)
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
    }

    public DateTime DueDate
    {
        get { return dueDate; }
        set
        {
            if (value != dueDate)
            {
                dueDate = value;
                OnPropertyChanged("DueDate");
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

    public int Priority
    {
        get { return priority; }
        set
        {
            if (value != priority)
            {
                priority = value;
                OnPropertyChanged("Priority");
            }
        }
    }

    public ObservableCollection<TodoSub> Subtasks
    {
        get { return subtasks; }
        set
        {
            if (value != subtasks)
            {
                subtasks = value;
                OnPropertyChanged("Subtasks");
            }
        }
    }

    public Todo()
    {
        subtasks = new ObservableCollection<TodoSub>();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
