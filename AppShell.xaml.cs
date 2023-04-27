using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;

namespace ShopWorld.MAUI;

public partial class AppShell : Shell
{
    private INavigationService _navigationService;
	public AppShell(INavigationService navigationService)
	{
		InitializeComponent();
        _navigationService = navigationService;
        Routing.RegisterRoute(nameof(ShoppingPage),typeof(ShoppingPage));
        Routing.RegisterRoute(nameof(StartUpPage),typeof(StartUpPage));
        Routing.RegisterRoute(nameof(LoginPage),typeof(LoginPage));
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}
