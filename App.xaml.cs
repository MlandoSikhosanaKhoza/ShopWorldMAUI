using ShopWorld.MAUI.Services;

namespace ShopWorld.MAUI;

public partial class App : Application
{
	public App(INavigationService navigationService)
	{
		InitializeComponent();

		MainPage = new AppShell(navigationService);
	}

}
