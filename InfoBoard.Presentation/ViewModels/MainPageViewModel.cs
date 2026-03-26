using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Domain.Models.Entities;

namespace InfoBoard.Presentation.ViewModels
{
    public partial class MainPageViewModel : BasePropertyChanged
    {
        private ToDoViewModel _todo = null!;

        private GameViewModel _game = null!;
        
        private WeatherViewModel _weather = null!;

        public GameViewModel Game 
        {
            get => _game; 
            set
            { 
                _game = value; OnPropertyChanged(nameof(Game)); 
            }
        }
        public ToDoViewModel ToDo
        {
            get => _todo;
            set 
            { 
                _todo = value; OnPropertyChanged(nameof(ToDo));
            }
        }
        public WeatherViewModel Weather
        {
            get => _weather; 
            set 
            { 
                _weather = value; OnPropertyChanged(nameof(Weather));
            }
        }

        public MainPageViewModel(ToDoViewModel todo, GameViewModel game, WeatherViewModel weather)
        {
            Weather = weather;
            ToDo = todo;
            Game = game;
        }
    }
}
