using System.Collections.ObjectModel;
using AHIFusion.View.Calendar;

namespace AHIFusion;

public sealed partial class MainPage : Page
{
    public ObservableCollection<TabViewItem> Tabs { get; set; }
    public MainPage()
    {
        this.InitializeComponent();

        Tabs = new ObservableCollection<TabViewItem>();
        Tabs.CollectionChanged += Tabs_CollectionChanged;
        AddHomeTab();

        var noteTab = AddTab(new NotesPage());
        noteTab.Header = "Notes";
        noteTab.IconSource = new SymbolIconSource { Symbol = Symbol.Edit };

        var noteTab1 = AddTab(new NotesPage());
        noteTab1.Header = "Notes";
        noteTab1.IconSource = new SymbolIconSource { Symbol = Symbol.Edit };

        var calendarTab1 = AddTab(new CalendarPage());
        calendarTab1.Header = "Calendar";
        calendarTab1.IconSource = new SymbolIconSource { Symbol = Symbol.Calendar };

        var clockTabe = AddTab(new ClockPage());
        clockTabe.Header = "Clock";
        clockTabe.IconSource = new SymbolIconSource { Symbol = Symbol.Clock };


        DataContext = this;
    }

    private TabViewItem AddTab(Page page)
    {
        TabViewItem tabViewItem = new TabViewItem()
        {
            IsClosable = false,
            Content = page,
        };
        Tabs.Add(tabViewItem);

        return tabViewItem;
    }

    private void AddHomeTab()
    {
        TabViewItem tab = AddTab(new HomePage());
        tab.Header = "Home";
        tab.IconSource = new SymbolIconSource { Symbol = Symbol.Home };
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
