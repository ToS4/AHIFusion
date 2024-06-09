using System.Collections.ObjectModel;
using AHIFusion.Model;
using Serilog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace AHIFusion
{
    public sealed partial class MainPage : Page
    {
        public static ObservableCollection<TabViewItem> Tabs { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()
               .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
               .CreateLogger();

            EventCollection.LoadFromFile("events.json");
            NoteCollection.LoadFromFile("notes.json");
            AlarmCollection.LoadFromFile("alarms.json");
            //StopwatchCollection.LoadFromFile("stopwatches.json");
            TimerCollection.LoadFromFile("timers.json");
            TodoCollection.LoadFromFile("todos.json");
            //StopwatchManager.Instance.LoadFromFile("stopwatch-managers.json");

            if (StopwatchCollection.Stopwatches.Count <= 0)
            {
                Stopwatch sw = new AHIFusion.Stopwatch()
                {
                    StartTime = new DateTime(0),
                    ElapsedTime = new TimeSpan(0),
                    IsRunning = false
                };

                StopwatchCollection.Stopwatches.Add(sw);
            }

            Tabs = new ObservableCollection<TabViewItem>();
            Tabs.CollectionChanged += Tabs_CollectionChanged;
            AddHomeTab();

            DataContext = this;

            // Start the welcome screen animation
            StartWelcomeAnimation();
        }

        private void StartWelcomeAnimation()
        {
            var storyboard = new Storyboard();

            var fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(TimeSpan.FromSeconds(2))
            };

            Storyboard.SetTarget(fadeOut, WelcomeScreen);
            Storyboard.SetTargetProperty(fadeOut, "Opacity");

            storyboard.Children.Add(fadeOut);

            storyboard.Completed += (s, e) =>
            {
                WelcomeScreen.Visibility = Visibility.Collapsed;
            };

            storyboard.Begin();
        }

        public static TabViewItem CreateTab(Page page)
        {
            TabViewItem tabViewItem = new TabViewItem()
            {
                IsClosable = false,
                Content = page,
            };

            return tabViewItem;
        }

        private void AddHomeTab()
        {
            TabViewItem tab = CreateTab(new HomePage());
            tab.Header = "Home";
            tab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };
            tab.IsSelected = true;
            Tabs.Add(tab);
        }

        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            AddHomeTab();
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            Tabs.Remove(args.Tab);
        }

        private void Tabs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            bool state = false;

            if (Tabs.Count > 1)
            {
                state = true;
            }

            foreach (TabViewItem item in Tabs)
            {
                item.IsClosable = state;
            }
        }
    }
}
