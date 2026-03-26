using InfoBoard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfoBoard.Core.Services
{
    public class YrWeatherFetcher : IYrWeatherFetcher
    {
        private readonly HttpClient _httpClient;

        public YrWeatherFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<List<JsonElement>> FetchTimeFrames(double lat, double lon)
        {
            string latStr = lat.ToString("F2", CultureInfo.InvariantCulture);
            string lonStr = lon.ToString("F2", CultureInfo.InvariantCulture);
            string url = $"https://api.met.no/weatherapi/locationforecast/2.0/compact?lat={latStr}&lon={lonStr}";

            var svar = await _httpClient.GetStringAsync(url);
            var dokument = JsonDocument.Parse(svar);

            return dokument.RootElement.GetProperty("properties").GetProperty("timeseries").EnumerateArray()
                      .Select(x => x.Clone())
                      .ToList();
        }

        public async Task<(string temp, string hum, string wind, string date)> FetchWeather(double lat, double lon)
        {
            try
            {
                var timeframes = await FetchTimeFrames(lat, lon);
                string today = DateTime.UtcNow.ToString("yyyy-MM-dd");

                var forsta = timeframes
                    .Select(item => new
                    {
                        TidStr = item.GetProperty("time").GetString(),
                        Detaljer = item.GetProperty("data").GetProperty("instant").GetProperty("details")
                    })
                    .FirstOrDefault(x => x.TidStr != null && x.TidStr.StartsWith(today));

                if (forsta == null)
                    return ("N/A", "N/A", "N/A", "N/A");

                return
                (
                    $"{forsta.Detaljer.GetProperty("air_temperature").GetSingle():F1}°C",
                    $"{forsta.Detaljer.GetProperty("relative_humidity").GetSingle():F0}%",
                    $"{forsta.Detaljer.GetProperty("wind_speed").GetSingle():F1} m/s",
                    DateTime.Parse(forsta.TidStr!).ToLocalTime().ToString("HH:mm")
                );
            }
            catch
            {
                return ("N/A", "N/A", "N/A", "N/A");
            }
        }
    }
}