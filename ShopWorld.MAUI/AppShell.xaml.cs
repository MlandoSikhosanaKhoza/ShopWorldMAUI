using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;

namespace ShopWorld.MAUI;

public partial class AppShell : Shell
{
	public AppShell(INavigationService navigationService)
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ShoppingPage),typeof(ShoppingPage));
		Routing.RegisterRoute(nameof(ShoppingCartPage),typeof(ShoppingCartPage));
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		//SecureStorage.RemoveAll();
		//if (System.IO.File.Exists(Constants.DatabasePath))
		//{
		//	System.IO.File.Delete(Constants.DatabasePath);
		//}
    }
}
