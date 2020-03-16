using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
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
        //public ObservableCollection<ObservableCollection<byte>> GameBoard { get; set; }


        private byte[,] GameBoardValue;
        public byte[,] GameBoard
        {
            get => GameBoardValue;
            set
            {
                GameBoardValue = value;
                OnPropertyChanged(nameof(GameBoard));
            }
        }


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
            Init();
        }

        private async void Init()
        {
            var isSuccess = await StartGameAsync();
            if (!isSuccess)
            {
                StatusMessage = "Game not started";
                return;
            }
            GetBoardAsync();
        }

        private async void GetBoardAsync()
        {
            var boardString = await HttpClient.GetStringAsync("/Connect4Game");
            System.Diagnostics.Debug.WriteLine(boardString);
            GameBoard = JsonConvert.DeserializeObject<byte[,]>(boardString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int row = int.Parse(((Button)sender).Tag.ToString());
            OnSetStoneAsync(row);
        }

        private async void OnSetStoneAsync(int? row)
        {
            var rowResponse = await HttpClient.PostAsync($"/Connect4Game/{row}", null);
            var content = await rowResponse.Content.ReadAsStringAsync();
            GetBoardAsync();
            if (content.Equals("1") || content.Equals("2"))
            {
                StatusMessage = $"Player {content} wins!";
                return;
            }
            if (!rowResponse.IsSuccessStatusCode)
            {
                StatusMessage = content;
                return;
            }
            StatusMessage = "";
        }

        private async Task<bool> StartGameAsync()
        {
            var startGameResponse = await HttpClient.PutAsync("/Connect4Game", null);

            return startGameResponse.IsSuccessStatusCode;
        }

    }
}
