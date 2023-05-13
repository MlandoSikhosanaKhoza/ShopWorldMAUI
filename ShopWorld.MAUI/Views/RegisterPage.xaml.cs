using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class RegisterPage : ContentPage
{
	private RegisterViewModel _viewModel;
	public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=_viewModel=viewModel;
	}

}