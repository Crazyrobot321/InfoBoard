using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Globalization;
using Domain.Models.Entities;
using InfoBoard.Core.Interfaces;
namespace InfoBoard.Core.Services
{
    public class CoordinatesManager : ICoordinatesManager
    {
        private readonly HttpClient _httpClient;    
        public CoordinatesManager(HttpClient httpClient) // Vi tar in HttpClient via DI
        {
            _httpClient = httpClient;
        }

        public async Task<(double lat, double lon)> FetchCoords(string cityname)
        {
            if (string.IsNullOrWhiteSpace(cityname)) // Enkel validering, inga onödig API-anrop
                return (0, 0);

            try
            {
                string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(cityname)}&format=jsonv2";

                string responseJson = await _httpClient.GetStringAsync(url);

                var coords = JsonSerializer.Deserialize<List<Coordinates>>(responseJson)
                    ?.Select(l => new
                    {
                        Lat = double.Parse(l.Lat, CultureInfo.InvariantCulture),
                        Lon = double.Parse(l.Lon, CultureInfo.InvariantCulture),
                    })
                    .FirstOrDefault();

                return coords != null ? (coords.Lat, coords.Lon) : (0, 0); // Om vi inte hittar några koordinater, returnera (0, 0)
            }
            catch (Exception ex)
            {
                // Felhantering krävs i listan!
                Debug.WriteLine($"Fel vid geokodning: {ex.Message}");
                return (0, 0);
            }
        }
    }
}
