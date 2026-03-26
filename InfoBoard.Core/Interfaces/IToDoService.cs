using Domain.Models.Entities;

namespace InfoBoard.Core.Interfaces
{
    public interface IToDoService
    {
        Task<List<TodoItem>> FetchToDoItems();
        Task AddToDoItem(string title);
        Task SaveToFile();
        Task RemoveFromFile(List<TodoItem> objekt);
    }
}