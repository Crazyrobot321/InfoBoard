using InfoBoard.Presentation.ViewModels;

namespace InfoBoard.Presentation.View;

public partial class ManageServers : ContentPage
{
	public ManageServers(ManageServersViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}