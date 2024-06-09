using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace AHIFusion;
public class TodoList : INotifyPropertyChanged
{
    private int id;
    private string name;
    private ObservableCollection<Todo> todos;
    private Color color;

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

    public string Name
    {
        get { return name; }
        set
        {
            if (value != name)
            {
                name = value; 
                OnPropertyChanged("Name");
            }

        }
    }

    public ObservableCollection<Todo> Todos
    {
        get { return todos; }
        set
        {
            if (value != todos)
            {
                todos = value;
                OnPropertyChanged("Todos");
            }
        }
    }

    public Color Color
    {
        get { return color; }
        set
        {
            if (value != color)
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }
    }

    public TodoList()
    {
        todos = new ObservableCollection<Todo>();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
