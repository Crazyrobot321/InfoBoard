using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Models.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace InfoBoard.Presentation.ViewModels
{
    public partial class GameViewModel : BasePropertyChanged
    {
        private readonly JsonSerializerOptions _options;

        private readonly string _filePath;

        private ObservableCollection<GameTimeItem> _allServers = new();
        public ObservableCollection<GameTimeItem> AllServers 
        {
            get => _allServers; 
            set 
            { 
                _allServers = value; OnPropertyChanged(nameof(AllServers)); 
            }
        }
        public IEnumerable<GameTimeItem> SelectedServers => AllServers.Where(s => s.IsSelected);

        public GameViewModel(string filePath, JsonSerializerOptions options)
        {
            _options = options;
            _filePath = filePath;
            _ = LoadServerData();

            // Timer for att uppdatera nedrakningen varje sekund
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += async (s, e) => await Countdown();
            timer.Start();
        }

        public async Task LoadServerData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    var objekt = JsonSerializer.Deserialize<List<GameTimeItem>>(json, _options);

                    if (objekt != null)
                    {
                        AllServers.Clear();
                        foreach (var server in objekt)
                        {
                            AllServers.Add(server);
                        }
                        OnPropertyChanged(nameof(SelectedServers));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while parsing json file: {ex}");
            }
        }

        public async Task SaveServerData()
        {
            if (AllServers == null || !AllServers.Any())
            {
                await Shell.Current.DisplayAlertAsync("No servers", "There are no servers to save.", "OK");
                return;
            }
            try
            {
                var json = JsonSerializer.Serialize(AllServers.ToList(), _options);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to save servers: {ex.Message}");
            }
        }

        private async Task Countdown()
        {
            DateTime now = DateTime.Now;

            await Task.Run(() =>
            {
                foreach (var item in SelectedServers)
                {
                    if (!item.IsSelected)
                        continue;

                    DateTime nextReset = item.BaseResetTime;

                    // Berakna nasta tillfalle baserat pa intervall
                    if (item.Interval != ResetInterval.Once)
                    {
                        while (nextReset < now)
                        {
                            if (item.Interval == ResetInterval.Daily)
                                nextReset = nextReset.AddDays(1);
                            else if (item.Interval == ResetInterval.Weekly)
                                nextReset = nextReset.AddDays(7);
                        }
                    }

                    TimeSpan diff = nextReset - now;

                    if (diff.TotalSeconds <= 0)
                    {
                        item.DisplayTime = "Resets now!";
                    }
                    else
                    {
                        if (diff.Days > 0)
                            item.DisplayTime = $"{diff.Days}d {diff.Hours}h {diff.Minutes}m";
                        else
                            item.DisplayTime = $"{diff.Hours}h {diff.Minutes}m {diff.Seconds}s";
                    }
                }
            });
        }

        public void UpdateView()
        {
            OnPropertyChanged(nameof(SelectedServers));
        }
    }
}