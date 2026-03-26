using InfoBoard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Domain.Models.Entities;
using InfoBoard.Core.Facade;
using System.Diagnostics;

namespace InfoBoard.Presentation.ViewModels
{
    public class WeatherViewModel : BasePropertyChanged
    {
        private readonly WeatherFacade _weatherFacade;

        private string _temp = "4°C";
        public string Temp { get => _temp; set { _temp = value; OnPropertyChanged(nameof(Temp)); } }

        private string _hum = "Humidity: --%";
        public string Hum { get => _hum; set { _hum = value; OnPropertyChanged(nameof(Hum)); } }

        private string _cityDisplay = "CITY";
        public string CityDisplay { get => _cityDisplay; set { _cityDisplay = value; OnPropertyChanged(nameof(CityDisplay)); } }

        private string _cityInputText = string.Empty;
        public string CityInputText { get => _cityInputText; set { _cityInputText = value; OnPropertyChanged(nameof(CityInputText)); } }

        public ICommand UpdateWeatherCommand { get; }

        public WeatherViewModel(WeatherFacade weatherFacade)
        {
            _weatherFacade = weatherFacade;
            UpdateWeatherCommand = new Command(async () => await UpdateWeather());
        }

        private async Task UpdateWeather()
        {
            string input = CityInputText?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(input) || !Regex.IsMatch(input, @"^[a-zA-ZåäöÅÄÖ\s-]+$"))
            {
                await Shell.Current.DisplayAlertAsync("Error",
                    "Enter a valid city name (only letters, whitespaces and dashes allowed).", "OK");
                return;
            }

            try
            {
                var (temp, hum, wind, date) = await _weatherFacade.GetWeather(input);

                CityDisplay = input.ToUpper();
                Temp = temp;
                Hum = "Humidity: " + hum;
                CityInputText = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to fetch weather: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Error", "Couldn't find city or any weather.", "OK");
            }
        }
    }
}
