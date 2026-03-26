using Domain.Models.Entities;
using InfoBoard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace InfoBoard.Presentation.ViewModels
{
    public class ToDoViewModel : BasePropertyChanged
    {
        private readonly IToDoService _todoService;
        private string _newToDoTitle = string.Empty;

        public ObservableCollection<TodoItem> Todos { get; } = new();

        public string NewToDoTitle { get => _newToDoTitle; set { _newToDoTitle = value; OnPropertyChanged(nameof(NewToDoTitle)); }}

        public ICommand AddCommand { get; }
        public ICommand RemoveFinishedCommand { get; }

        public ToDoViewModel(IToDoService todoService)
        {
            _todoService = todoService;
            AddCommand = new Command(async () => await AddToDo());
            RemoveFinishedCommand = new Command(async () => await RemoveFinished());
            _ = LoadData();
        }

        private async Task LoadData()
        {
            var objekt = await _todoService.FetchToDoItems();
            Todos.Clear();
            foreach (var item in objekt)
                Todos.Add(item);

            UpdateProgress();
        }

        public async Task AddToDo()
        {
            if (string.IsNullOrWhiteSpace(NewToDoTitle))
                return;

            await _todoService.AddToDoItem(NewToDoTitle);
            var all = await _todoService.FetchToDoItems();

            if (all.Any())
            {
                Todos.Add(all.Last());
                NewToDoTitle = string.Empty;
                UpdateProgress();
            }
        }

        public async Task SaveStatus()
        {
            await _todoService.SaveToFile();
        }

        public double ToDoProgress
        {
            get
            {
                if (Todos.Count == 0)
                    return 0;

                double done = Todos.Count(t => t.IsCompleted);
                return done / Todos.Count;
            }
        }

        public string ProgressText => $"Progress: {Math.Round(ToDoProgress * 100)}%";

        public void UpdateProgress()
        {
            OnPropertyChanged(nameof(ToDoProgress));
            OnPropertyChanged(nameof(ProgressText));
        }

        public async Task RemoveFinished()
        {
            var ToDos_ToKeep = Todos.Where(t => !t.IsCompleted).ToList();

            Todos.Clear();
            foreach (var item in ToDos_ToKeep)
            {
                Todos.Add(item);
            }

            await _todoService.RemoveFromFile(ToDos_ToKeep);
            UpdateProgress();
        }
    }
}