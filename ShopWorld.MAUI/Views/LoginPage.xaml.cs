using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _viewModel;
    public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this,false);
		BindingContext=_viewModel=viewModel;
	}

	

}