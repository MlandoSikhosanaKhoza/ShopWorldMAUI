using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Services;

namespace ShopWorld.MAUI;

public partial class App : Application
{
	public App(INavigationService navigationService,IUnitOfWork unitOfWork)
	{
		InitializeComponent();

		MainPage = new AppShell(navigationService,unitOfWork);
	}

}
