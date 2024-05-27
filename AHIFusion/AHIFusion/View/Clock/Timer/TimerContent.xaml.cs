using System.Collections.Specialized;
using AHIFusion.Model;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Animation;
using Uno;

namespace AHIFusion;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TimerContent : Page
{
    public TimerContent()
    {
        this.InitializeComponent();
        DataContext = this;
        InitializeControls();
        TimerCollection.Timers.CollectionChanged += Timers_CollectionChanged;
    }

    private void InitializeControls()
    {
        for (int i = 0; i < TimerCollection.Timers.Count; i++)
        {
            TimerControl timerControl = new TimerControl
            {
                DataContext = TimerCollection.Timers[i],
                Margin = new Thickness(10),
                MaxHeight = 320,
                MinHeight = 320
            };

            Binding titleBinding = new Binding
            {
                Path = new PropertyPath("Title"),
                Mode = BindingMode.TwoWay
            };

            Binding timeBinding = new Binding
            {
                Path = new PropertyPath("Time"),
                Mode = BindingMode.TwoWay
            };

            Binding isRunningBinding = new Binding
            {
                Path = new PropertyPath("IsRunning"),
                Mode = BindingMode.TwoWay
            };

            Binding soundBinding = new Binding
            {
                Path = new PropertyPath("Sound"),
                Mode = BindingMode.TwoWay
            };

            timerControl.SetBinding(TimerControl.TitleProperty, titleBinding);
            timerControl.SetBinding(TimerControl.TimeProperty, timeBinding);
            timerControl.SetBinding(TimerControl.IsRunningProperty, isRunningBinding);
            timerControl.SetBinding(TimerControl.SoundProperty, soundBinding);

            int row = i / 2;
            int column = i % 2;

            if (row >= MainGrid.RowDefinitions.Count)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star)});
            }

            Grid.SetRow(timerControl, row);
            Grid.SetColumn(timerControl, column);

            MainGrid.Children.Add(timerControl);

            for (int j = MainGrid.RowDefinitions.Count - 1; j >= 0; j--)
            {
                bool rowOccupied = MainGrid.Children.Cast<UIElement>().Any(e => Grid.GetRow((FrameworkElement)e) == j);
                if (!rowOccupied)
                {
                    MainGrid.RowDefinitions.RemoveAt(j);
                }
            }

            
        }

        AddRectControl addRectControl = new AddRectControl
        {
            Margin = new Thickness(10),
            MaxHeight = 320,
            MinHeight = 320,
            Mode = "Timer"
        };

        int addRectRow = TimerCollection.Timers.Count / 2;
        int addRectColumn = TimerCollection.Timers.Count % 2;

        if (addRectRow >= MainGrid.RowDefinitions.Count)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        Grid.SetRow(addRectControl, addRectRow);
        Grid.SetColumn(addRectControl, addRectColumn);

        MainGrid.Children.Add(addRectControl);
    }

    private void Timers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (AHIFusion.Model.Timer timer in e.NewItems)
            {
                AddTimerControl(timer);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            InitializeControls();
        }
    }

    private void AddTimerControl(AHIFusion.Model.Timer timer)
    {
        var addRectControl = MainGrid.Children.OfType<AddRectControl>().FirstOrDefault();
        if (addRectControl != null)
        {
            MainGrid.Children.Remove(addRectControl);
        }

        if (MainGrid.Children.OfType<TimerControl>().Count() % 2 == 0)
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        TimerControl timerControl = new TimerControl
        {
            DataContext = timer,
            Margin = new Thickness(10),
            MaxHeight = 320,
            MinHeight = 320
        };

        Binding titleBinding = new Binding
        {
            Path = new PropertyPath("Title"),
            Mode = BindingMode.TwoWay
        };

        Binding timeBinding = new Binding
        {
            Path = new PropertyPath("Time"),
            Mode = BindingMode.TwoWay
        };

        Binding isRunningBinding = new Binding
        {
            Path = new PropertyPath("IsRunning"),
            Mode = BindingMode.TwoWay
        };

        Binding soundBinding = new Binding
        {
            Path = new PropertyPath("Sound"),
            Mode = BindingMode.TwoWay
        };

        timerControl.SetBinding(TimerControl.TitleProperty, titleBinding);
        timerControl.SetBinding(TimerControl.TimeProperty, timeBinding);
        timerControl.SetBinding(TimerControl.IsRunningProperty, isRunningBinding);
        timerControl.SetBinding(TimerControl.SoundProperty, soundBinding);

        int i = MainGrid.Children.OfType<TimerControl>().Count();

        int row = i / 2;
        int column = i % 2;

        if (row >= MainGrid.RowDefinitions.Count)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        Grid.SetRow(timerControl, row);
        Grid.SetColumn(timerControl, column);

        MainGrid.Children.Add(timerControl);

        var animation = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = new Duration(TimeSpan.FromSeconds(0.5))
        };

        var storyboard = new Storyboard();
        Storyboard.SetTarget(animation, timerControl);
        Storyboard.SetTargetProperty(animation, "Opacity");
        storyboard.Children.Add(animation);

        storyboard.Begin();

        if (addRectControl != null)
        {
            int addRectRow = MainGrid.Children.OfType<TimerControl>().Count() / 2;
            int addRectColumn = MainGrid.Children.OfType<TimerControl>().Count() % 2;

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
