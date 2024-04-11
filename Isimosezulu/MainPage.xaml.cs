using System.Windows.Input;
using Newtonsoft.Json;
using Isimosezulu.Models;


namespace Isimosezulu
{
    public partial class MainPage : ContentPage
    {
        private float _temperature;

        public float Temperature
        {
            get { return _temperature; }
            set { _temperature = value; OnPropertyChanged(); }

        }
        private HttpClient _client;

        public MainPage()
        {
            InitializeComponent();
            
            BindingContext = this;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

        }

        public async void GetLatestJoke(object parameters)
        {
            string response = await _client.GetStringAsync(new Uri("https://icanhazdadjoke.com/"));

            Rootobject latestJoke = JsonConvert.DeserializeObject<Rootobject>(response);

            if (latestJoke != null)
            {
                Temperature = latestJoke.main.temp;
            }
        }
    }

}


        /*public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }*/
    


