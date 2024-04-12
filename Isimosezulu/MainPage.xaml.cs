using System.Windows.Input;
using Newtonsoft.Json;
using Isimosezulu.Models;


namespace Isimosezulu
{
    public partial class MainPage : ContentPage
    {
        private float _temperature;
        private float _pressure;
        private int _humidity;

        public float Temperature
        {
            get { return _temperature; }
            set { _temperature = value; OnPropertyChanged(); }

        }

        public float Pressure
        {
            get { return _pressure; }
            set { _pressure = value; OnPropertyChanged();}

        }
        public int Humidity
        {
            get { return _humidity; }
            set { _humidity = value; OnPropertyChanged(); }
        }
        private HttpClient _client;

        public MainPage()
        {
            InitializeComponent();
            
            BindingContext = this;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            GetWeather(_client);
            GetCurrentLocation();
        }

        public async void GetWeather(object parameters)
        {
            string response = await _client.GetStringAsync(new Uri("https://api.openweathermap.org/data/2.5/weather?lat=44.34&lon=10.99&appid=7df1acb50a13068da6bae9428db0aa20&units=metric"));

            Rootobject GetWeather = JsonConvert.DeserializeObject<Rootobject>(response);

            if (GetWeather != null)
            {
                Temperature = GetWeather.main.temp;
                Pressure = GetWeather.main.pressure;
                Humidity = GetWeather.main.humidity;
            }
        }
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                {
                    latitude = location.Latitude;
                    longitude = location.Longitude;
                    CurrentLocation = $"Latitude: {latitude}, Longitude: {longitude}";

                    GetWeather(null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location: {ex.Message}");
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && !_cancelTokenSource.IsCancellationRequested)
                _cancelTokenSource.Cancel();
        }
        private double latitude;
        private double longitude;

        private string _currentLocation;
        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; OnPropertyChanged(); }
        }

        public async void GetWeatherForCity(string city)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat=44.34&lon=10.99&appid=7df1acb50a13068da6bae9428db0aa20&units=metric");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Rootobject GetWeather = JsonConvert.DeserializeObject<Rootobject>(responseBody);

                if (GetWeather != null)
                {
                    Temperature = GetWeather.main.temp;
                    Humidity = GetWeather.main.humidity;
                    Pressure = GetWeather.main.pressure;
                    CurrentLocation = $"City: {GetWeather.name}, Country: {GetWeather.sys.country}";
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error getting weather data: {ex.Message}");
            }
        }
    }

}


       