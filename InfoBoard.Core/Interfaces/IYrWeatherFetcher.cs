namespace InfoBoard.Core.Interfaces
{
    public interface IYrWeatherFetcher
    {
        Task<(string temp, string hum, string wind, string date)> FetchWeather(double lat, double lon);
    }
}