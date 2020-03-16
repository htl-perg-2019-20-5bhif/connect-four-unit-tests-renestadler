using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ConnectFour.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<ObservableCollection<byte>> GameBoard { get; set; }


        private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://connectfourapi.azurewebsites.net"),
            Timeout = TimeSpan.FromSeconds(500)
        };

        private string StatusMessageValue;
        public string StatusMessage
        {
            get => StatusMessageValue;
            set
            {
                StatusMessageValue = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitAsync();
        }

        /*private void convertArrayToObservableCollection(byte[,] arr)
        {
            ObservableCollection<ObservableCollection<byte>> collection = new ObservableCollection<ObservableCollection<byte>>();
            for(int row = 0; row < 6; i++)
            {
                collection.Add(new ObservableCollection<byte>());
                for(int col = 0; col < 7; col++)
                {
                    collection[row].Add(arr[row, col]);
                }
            }
        }*/

        private async void InitAsync()
        {
            System.Diagnostics.Debug.WriteLine($"Init ##############");
            var isSuccess = await StartGameAsync();
            System.Diagnostics.Debug.WriteLine($"Success: {isSuccess} ##############");
            if (!isSuccess)
            {
                System.Diagnostics.Debug.WriteLine($"Game not started ##############");
                StatusMessage = "Game not started";
                    return;
            }
            System.Diagnostics.Debug.WriteLine($"Game started ##############");
            GetBoardAsync();
        }

        private async void GetBoardAsync()
        {
            //HttpClient.asdf
            var boardString = await HttpClient.GetStringAsync("/Connect4Game");
            GameBoard = JsonSerializer.Deserialize<ObservableCollection<ObservableCollection<byte>>>(boardString);

            System.Diagnostics.Debug.WriteLine($"Board: {boardString} ##############");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int row = int.Parse(((Button)sender).Tag.ToString());
            OnSetStoneAsync(row);
        }

        private async void OnSetStoneAsync(int? row)
        {
            //var rowDTO = new RowDTO { row = row }();
            //var content = new StringContent(JsonSerializer.Serialize(rowDTO), Encoding.UTF8, "application/json");
            var rowResponse = await HttpClient.PostAsync($"/Connect4Game/{row}", null);
            GetBoardAsync();
            if (!rowResponse.IsSuccessStatusCode)
            {
                StatusMessage = rowResponse.ReasonPhrase;
            }
            //pokemonResponse.EnsureSuccessStatusCode();
        }

        private async Task<bool> StartGameAsync()
        {
            var startGameResponse = await HttpClient.PutAsync("/Connect4Game", null);

            return startGameResponse.IsSuccessStatusCode;
        }

    }
}
