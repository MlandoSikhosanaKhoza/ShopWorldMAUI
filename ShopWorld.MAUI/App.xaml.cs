using ShopWorld.MAUI.Services;

namespace ShopWorld.MAUI;

public partial class App : Application
{
	public App(INavigationService navigationService,IAuthorizationService authorizationService)
	{
		InitializeComponent();
		//If the program doesn't start up use this method (Uncomment the method)
		//authorizationService.WipePersonalDataAsync();
		MainPage = new AppShell(navigationService);
	}
}
