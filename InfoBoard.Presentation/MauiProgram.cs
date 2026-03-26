using InfoBoard.Core.Facade;
using InfoBoard.Core.Interfaces;
using InfoBoard.Core.Services;
using InfoBoard.Presentation.View;
using InfoBoard.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InfoBoard.Presentation
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // 1. FILE PATHS AND BASE SETTINGS
            string todoFilePath = Path.Combine(FileSystem.AppDataDirectory, "todos.json");
            string gamePath = Path.Combine(FileSystem.AppDataDirectory, "gameservers.json");

            builder.Services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            });

            // 2. HTTP CLIENTS
            string userAgent = "InfoBoardApp (vwcurwbg7@mozmail.com)";

            builder.Services.AddHttpClient<ICoordinatesManager, CoordinatesManager>(client =>
                client.DefaultRequestHeaders.Add("User-Agent", userAgent));

            builder.Services.AddHttpClient<IYrWeatherFetcher, YrWeatherFetcher>(client =>
                client.DefaultRequestHeaders.Add("User-Agent", userAgent));

            builder.Services.AddHttpClient<IRssService, RssService>(client =>
                client.DefaultRequestHeaders.Add("User-Agent", userAgent));

            // 3. SERVICES
            builder.Services.AddSingleton<IToDoService>(todo =>
            {
                var alternativ = todo.GetRequiredService<JsonSerializerOptions>();
                return new ToDoService(todoFilePath, alternativ);
            });

            builder.Services.AddTransient<WeatherFacade>();
            // 4. VIEWMODELS
            builder.Services.AddSingleton<ToDoViewModel>();
            builder.Services.AddSingleton<GameViewModel>(s =>
            {
                var alternativ = s.GetRequiredService<JsonSerializerOptions>();
                return new GameViewModel(gamePath, alternativ);
            });

            builder.Services.AddTransient<WeatherViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ManageServersViewModel>();

            // 5. Views (UI)
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ManageServers>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
