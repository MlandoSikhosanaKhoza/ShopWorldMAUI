using ShopWorld.MAUI.Views;

namespace ShopWorld.MAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ShoppingPage),typeof(ShoppingPage));
	}
}
