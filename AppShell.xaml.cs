using ShopWorld.MAUI.Services;

namespace ShopWorld.MAUI;

public partial class AppShell : Shell
{
	public AppShell(INavigationService navigationService)
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}
