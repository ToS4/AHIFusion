using System.Drawing;
using System.Text.Json;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Devices.Geolocation;

namespace AHIFusion
{
	public sealed partial class HomePage : Page
	{
        private const string OpenWeatherMapApiKey = "e7c7b443582143eb2bdb819309c83af4";
        private const string OpenWeatherMapApiUrl = "http://api.openweathermap.org/data/2.5/weather";

        public HomePage()
		{
			this.InitializeComponent();
            LocateAndLoadWeatherData();
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
    }
}
