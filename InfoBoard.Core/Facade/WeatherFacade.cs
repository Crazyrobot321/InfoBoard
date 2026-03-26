using InfoBoard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoBoard.Core.Facade
{
    public class WeatherFacade
    {
        private readonly ICoordinatesManager _geoService;
        private readonly IYrWeatherFetcher _weatherService;

        public WeatherFacade(ICoordinatesManager geoService, IYrWeatherFetcher weatherService)
        {
            _geoService = geoService;
            _weatherService = weatherService;
        }

        public async Task<(string temp, string hum, string wind, string time)> GetWeather(string city)
        {
            // Steg 1: Omvandla stad till koordinater via Geo-tjänsten
            var coords = await _geoService.FetchCoords(city);

            if (coords.lat == 0 && coords.lon == 0)
                return ("N/A", "N/A", "N/A", "N/A");

            // Steg 2: Använd koordinaterna för att hämta vädret från din Yr-tjänst
            var weather = await _weatherService.FetchWeather(coords.lat, coords.lon);

            return (weather.temp, weather.hum, weather.wind, weather.date);
        }
    }
}
