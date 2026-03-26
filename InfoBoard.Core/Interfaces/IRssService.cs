using Domain.Models.Entities;

namespace InfoBoard.Core.Interfaces
{
    public interface IRssService
    {
        Task<List<RssItem>> GetSvtNews();
    }
}