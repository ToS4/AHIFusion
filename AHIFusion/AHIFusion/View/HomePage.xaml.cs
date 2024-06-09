using System.Text.Json;
using AHIFusion.Model;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Devices.Geolocation;
using Windows.UI;

namespace AHIFusion
{
	public sealed partial class HomePage : Page
	{
        private const string OpenWeatherMapApiKey = "e7c7b443582143eb2bdb819309c83af4";
        private const string OpenWeatherMapApiUrl = "http://api.openweathermap.org/data/2.5/weather";

        public string theme;
        public HomePage()
		{
			this.InitializeComponent();
            LocateAndLoadWeatherData();

            theme = ThemeConfig.theme;

            this.Loaded += HomePage_Loaded;

        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            theme = ThemeConfig.theme;
            FrameworkElement rootElement = XamlRoot.Content as FrameworkElement;
            if (theme == "Dark")
            {
                rootElement.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                rootElement.RequestedTheme = ElementTheme.Light;
            }

            var lighterColor = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[theme])["TertiaryContainerColor"];
            SettingsStackPanel.Background = new SolidColorBrush(lighterColor);
        }

        private async void LocateAndLoadWeatherData()
        {
            var geoLocator = new Geolocator();
            var position = await geoLocator.GetGeopositionAsync();
            var latitude = position.Coordinate.Point.Position.Latitude;
            var longitude = position.Coordinate.Point.Position.Longitude;

            await LoadWeatherData(latitude, longitude);
        }

        private async Task LoadWeatherData(double latitude, double longitude)
        {
            string url = $"{OpenWeatherMapApiUrl}?lat={latitude}&lon={longitude}&appid={OpenWeatherMapApiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var weatherData = JsonDocument.Parse(response);

                var cityName = weatherData.RootElement.GetProperty("name").GetString();
                var weatherDescription = weatherData.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();
                var iconCode = weatherData.RootElement.GetProperty("weather")[0].GetProperty("icon").GetString();
                var temperature = weatherData.RootElement.GetProperty("main").GetProperty("temp").GetDouble();

                CityTextBlock.Text = cityName;
                WeatherDescriptionTextBlock.Text = weatherDescription;
                TemperatureTextBlock.Text = $"{temperature}°C";

                string iconUrl = $"http://openweathermap.org/img/wn/{iconCode}.png";
                WeatherIcon.Source = new BitmapImage(new Uri(iconUrl));
            }
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

        private void StackPanel_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            var darkerColor = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[theme])["TertiaryContainerColorDarker"];
            SettingsStackPanel.Background = new SolidColorBrush(darkerColor);
        }

        private void StackPanel_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            var lighterColor = (Color)((ResourceDictionary)this.Resources.MergedDictionaries[0].ThemeDictionaries[theme])["TertiaryContainerColor"];
            SettingsStackPanel.Background = new SolidColorBrush(lighterColor);
        }

        private void StackPanel_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var content = XamlRoot.Content as FrameworkElement;
            content.RequestedTheme = content.RequestedTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
            theme = content.RequestedTheme == ElementTheme.Dark ? "Dark" : "Light";
            ThemeConfig.theme = theme;
        }
    }
}
