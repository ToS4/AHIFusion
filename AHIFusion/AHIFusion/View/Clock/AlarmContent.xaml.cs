using AHIFusion.Model;
using Windows.Graphics.Capture;

namespace AHIFusion;

using System.Collections.Specialized;
using AHIFusion.Model;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Uno;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public partial class AlarmContent : Page
{
    public AlarmContent()
    {
        this.InitializeComponent();

        DataContext = this;

        InitializeControls();

        AlarmCollection.Alarms.CollectionChanged += Alarms_CollectionChanged;
        
    }

    private void InitializeControls()
    {

        for (int i = 0; i < AlarmCollection.Alarms.Count; i++)
        {
            AlarmControl alarmControl = new AlarmControl
            {
                DataContext = AlarmCollection.Alarms[i],
                Margin = new Thickness(10),
                MaxHeight = 320,
                MinHeight = 320
            };

            Binding timeBinding = new Binding
            {
                Path = new PropertyPath("Time"),
                Mode = BindingMode.TwoWay
            };

            Binding titleBinding = new Binding
            {
                Path = new PropertyPath("Title"),
                Mode = BindingMode.TwoWay
            };

            Binding isOnBinding = new Binding
            {
                Path = new PropertyPath("IsOn"),
                Mode = BindingMode.TwoWay
            };

            Binding daysBinding = new Binding
            {
                Path = new PropertyPath("Days"),
                Mode = BindingMode.TwoWay
            };

            alarmControl.SetBinding(AlarmControl.TimeProperty, timeBinding);
            alarmControl.SetBinding(AlarmControl.TitleProperty, titleBinding);
            alarmControl.SetBinding(AlarmControl.IsOnProperty, isOnBinding);
            alarmControl.SetBinding(AlarmControl.DaysProperty, daysBinding);

            int row = i / 2;
            int column = i % 2;

            if (row >= MainGrid.RowDefinitions.Count)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            Grid.SetRow(alarmControl, row);
            Grid.SetColumn(alarmControl, column);

            MainGrid.Children.Add(alarmControl);

            for (int j = MainGrid.RowDefinitions.Count - 1; j >= 0; j--)
            {
                bool rowIsOccupied = MainGrid.Children.Cast<UIElement>().Any(e => Grid.GetRow((FrameworkElement)e) == j);

                if (!rowIsOccupied)
                {
                    MainGrid.RowDefinitions.RemoveAt(j);
                }
            }
        }

        AddRectControl addRectControl = new AddRectControl
        {
            Margin = new Thickness(10),
            MaxHeight = 320,
            MinHeight = 320
        };

        int addRectRow = AlarmCollection.Alarms.Count / 2;
        int addRectColumn = AlarmCollection.Alarms.Count % 2;

        if (addRectRow >= MainGrid.RowDefinitions.Count)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        Grid.SetRow(addRectControl, addRectRow);
        Grid.SetColumn(addRectControl, addRectColumn);

        MainGrid.Children.Add(addRectControl);
    }

    private void Alarms_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            foreach (Alarm newAlarm in e.NewItems)
            {
                AddAlarmControl(newAlarm);
            }
        }
    }

    private void AddAlarmControl(Alarm alarm)
    {
        // diesser Abschnitt löscht das AddRectControl, wenn es existiert (später wirds als letztes Element
        // zurück hinzugefügt)
        var addRectControl = MainGrid.Children.OfType<AddRectControl>().FirstOrDefault();
        if (addRectControl != null)
        {
            MainGrid.Children.Remove(addRectControl);
        }

        // das AlarmControl wird hinzugefügt
        MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AlarmControl alarmControl = new AlarmControl
        {
            DataContext = alarm,
            Margin = new Thickness(10),
            MaxHeight = 320,
            MinHeight = 320
        };

        Binding timeBinding = new Binding
        {
            Path = new PropertyPath("Time"),
            Mode = BindingMode.TwoWay
        };

        Binding titleBinding = new Binding
        {
            Path = new PropertyPath("Title"),
            Mode = BindingMode.TwoWay
        };

        Binding isOnBinding = new Binding
        {
            Path = new PropertyPath("IsOn"),
            Mode = BindingMode.TwoWay
        };

        Binding daysBinding = new Binding
        {
            Path = new PropertyPath("Days"),
            Mode = BindingMode.TwoWay
        };

        alarmControl.SetBinding(AlarmControl.TimeProperty, timeBinding);
        alarmControl.SetBinding(AlarmControl.TitleProperty, titleBinding);
        alarmControl.SetBinding(AlarmControl.IsOnProperty, isOnBinding);
        alarmControl.SetBinding(AlarmControl.DaysProperty, daysBinding);

        int i = MainGrid.Children.OfType<AlarmControl>().Count();

        int row = i / 2;
        int column = i % 2;

        if (row >= MainGrid.RowDefinitions.Count)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        Grid.SetRow(alarmControl, row);
        Grid.SetColumn(alarmControl, column);

        MainGrid.Children.Add(alarmControl);

        // Animation für das Hinzufügen des AlarmControls
        var animation = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = new Duration(TimeSpan.FromSeconds(0.5))
        };

        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, alarmControl);
        Storyboard.SetTargetProperty(animation, "Opacity");
        storyboard.Children.Add(animation);

        storyboard.Begin();

        // das AddRectControl wird als letztes Element hinzugefügt

        if (addRectControl != null)
        {
            int addRectRow = MainGrid.Children.OfType<AlarmControl>().Count() / 2;
            int addRectColumn = MainGrid.Children.OfType<AlarmControl>().Count() % 2;

            if (addRectRow >= MainGrid.RowDefinitions.Count)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            Grid.SetRow(addRectControl, addRectRow);
            Grid.SetColumn(addRectControl, addRectColumn);

            MainGrid.Children.Add(addRectControl);
        }
    }
}
