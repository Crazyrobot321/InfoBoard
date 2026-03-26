namespace InfoBoard.Core.Interfaces
{
    public interface ICoordinatesManager
    {
        Task<(double lat, double lon)> FetchCoords(string cityname);
    }
}