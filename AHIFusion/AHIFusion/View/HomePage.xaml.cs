using System.Drawing;

namespace AHIFusion
{
	public sealed partial class HomePage : Page
	{
		public HomePage()
		{
			this.InitializeComponent();
		}

        private void ReplaceTab(Page page, string header, SymbolIconSource icon)
        {
            TabViewItem tab = MainPage.CreateTab(page);
            tab.Header = header;
            tab.IconSource = icon;
            tab.IsSelected = true;

            foreach (TabViewItem tabViewItem in MainPage.Tabs)
            {
                if (tabViewItem.Content == this)
                {
                    int index = MainPage.Tabs.IndexOf(tabViewItem);

                    if (index != -1)
                    {
                        MainPage.Tabs[index] = tab;
                    }
                    else
                    {
                        Console.WriteLine("Object to replace not found in the list.");
                    }

                    break;
                }
            }
        }

        private void Todo_Click(object sender, RoutedEventArgs e)
        {
            ReplaceTab(
                new TodoPage(), 
                "Todo", 
                new SymbolIconSource { Symbol = Symbol.Go }
                );
        }

        private void Calendar_Click(object sender, RoutedEventArgs e)
        {
            ReplaceTab(
                new CalendarPage(),
                "Calendar",
                new SymbolIconSource { Symbol = Symbol.Calendar }
                );
        }

        private void Notes_Click(object sender, RoutedEventArgs e)
        {
            ReplaceTab(
                new NotesPage(),
                "Notes",
                new SymbolIconSource { Symbol = Symbol.Edit }
                );
        }

        private void Clock_Click(object sender, RoutedEventArgs e)
        {
            ReplaceTab(
                new ClockPage(),
                "Clock",
                new SymbolIconSource { Symbol = Symbol.Clock }
                );
        }
    }
}
