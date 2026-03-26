using Domain.Models.Entities;
using InfoBoard.Core;
using InfoBoard.Core.Interfaces;
using InfoBoard.Presentation.View;
using InfoBoard.Presentation.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace InfoBoard.Presentation
{
    public partial class MainPage : ContentPage
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        private readonly ICoordinatesManager _coordinatesManager;
        private readonly IYrWeatherFetcher _yrWeatherFetcher;
        private readonly MainPageViewModel _vm;
        private readonly IRssService _rssService;

        public MainPage(ICoordinatesManager coordinatesManager, IYrWeatherFetcher yrWeatherFetcher, MainPageViewModel MainPage, IRssService rssService)
        {
            _coordinatesManager = coordinatesManager;
            _yrWeatherFetcher = yrWeatherFetcher;
            _vm = MainPage;
            BindingContext = _vm;
            _rssService = rssService;

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadRssAsync();
        }

        private async void OnClickUpdateFile(object sender, CheckedChangedEventArgs e)
        {
            if (_vm != null)
            {
                await _vm.ToDo.SaveStatus();
                _vm.ToDo.UpdateProgress();
            }
        }

        private async void OnClickGoManage(object sender, EventArgs e)
        {
            var serviceProvider = IPlatformApplication.Current.Services;
            var page = serviceProvider.GetRequiredService<ManageServers>();
            await Navigation.PushAsync(page);
        }

        private async Task LoadRssAsync()
        {
            RssRefreshView.IsRefreshing = true;
            var nyheter = await _rssService.GetSvtNews();
            RssListView.ItemsSource = nyheter;
            RssRefreshView.IsRefreshing = false;
        }

        private async void OnRssSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.CurrentSelection.FirstOrDefault() as RssItem;

            if (selectedItem != null && !string.IsNullOrEmpty(selectedItem.Link))
            {
                try
                {
                    await Launcher.Default.OpenAsync(selectedItem.Link);
                }
                catch (Exception)
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Couldn't open the link.", "OK");
                }
                finally
                {
                    ((CollectionView)sender).SelectedItem = null;
                }
            }
        }
    }
}