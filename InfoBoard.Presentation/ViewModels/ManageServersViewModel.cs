using Domain.Models.Entities;
using System.Windows.Input;

namespace InfoBoard.Presentation.ViewModels;

public partial class ManageServersViewModel : BasePropertyChanged
{
    public GameViewModel Servers { get; }

    private NewServer _serverForm = new();
    public NewServer ServerForm 
    { 
        get => _serverForm; 
        set 
        { 
            _serverForm = value; OnPropertyChanged(nameof(ServerForm));
        }
    }

    public List<ResetInterval> Intervals { get; } = Enum.GetValues<ResetInterval>().ToList();
    public ICommand AddServerCommand { get; }
    public ICommand SaveListCommand { get; }
    public ICommand RemoveSelectedCommand { get; }

    public ManageServersViewModel(GameViewModel gameViewModel)
    {
        Servers = gameViewModel;
        AddServerCommand = new Command(async () => await AddNewServer());
        SaveListCommand = new Command(async () => await SaveList());
        RemoveSelectedCommand = new Command(async () => await RemoveSelected());
    }
    private async Task AddNewServer()
    {
        var newServer = new GameTimeItem
        {
            Game = ServerForm.GameName,
            Name = ServerForm.ResetName,
            BaseResetTime = ServerForm.Date.Date + ServerForm.Time,
            Interval = ServerForm.Interval,
            IsSelected = true
        };

        Servers.AllServers.Add(newServer);

        await Servers.SaveServerData();
        await SaveList();

        ServerForm = new NewServer(); //Empty the form
    }

    private async Task SaveList()
    {
        await Servers.SaveServerData();
        Servers.UpdateView();
        await Shell.Current.DisplayAlertAsync("Saved", "Serverlist saved!", "OK");
    }
    private async Task RemoveSelected()
    {
        var toRemove = Servers.AllServers.Where(s => s.IsSelected).ToList();

        if (toRemove.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Info", "Check the servers to be removed.", "OK");
            return;
        }

        bool confirm = await Shell.Current.DisplayAlertAsync("Remove",
            $"Are you sure you want to remove {toRemove.Count} servers?", "Yes", "No");

        if (confirm)
        {
            foreach (var server in toRemove)
            {
                Servers.AllServers.Remove(server);
            }

            await Servers.SaveServerData();

            Servers.UpdateView();
        }
    }
}