using Domain.Models.Entities;
using InfoBoard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfoBoard.Core.Services
{
    public class ToDoService : IToDoService
    {
        public JsonSerializerOptions _options;
        private readonly string _filePath;
        private List<TodoItem> _todos = new();

        public ToDoService(string filePath, JsonSerializerOptions options)
        {
            _filePath = filePath;
            string mappSokvag = Path.GetDirectoryName(_filePath) ?? string.Empty;

            if (!Directory.Exists(mappSokvag))
            {
                Directory.CreateDirectory(mappSokvag);
            }

            _options = options;
        }

        public async Task AddToDoItem(string titel)
        {
            if (string.IsNullOrWhiteSpace(titel))
                return;

            var nyttObjekt = new TodoItem { Title = titel, IsCompleted = false };
            _todos.Add(nyttObjekt);
            await SaveToFile();
        }

        public async Task<List<TodoItem>> FetchToDoItems()
        {
            if (_todos.Any()) // Returnera cachade todos om de redan är laddade
                return _todos;

            if (!File.Exists(_filePath)) // Om filen inte finns, returnera tom lista
                return new List<TodoItem>();

            try
            {
                using var stream = File.OpenRead(_filePath);
                _todos = await JsonSerializer.DeserializeAsync<List<TodoItem>>(stream, _options) ?? new();
            }
            catch
            {
                _todos = new List<TodoItem>(); // Vid fel, initiera tom lista
            }

            return _todos;
        }

        public async Task SaveToFile()
        {
            using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, _todos, _options);
        }

        public async Task RemoveFromFile(List<TodoItem> objekt)
        {
            try
            {
                // Här uppdaterar vi den lokala listan och sparar ner allt
                _todos = objekt;
                var json = JsonSerializer.Serialize(_todos, _options);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid skrivning till fil: {ex.Message}");
            }
        }
    }
}