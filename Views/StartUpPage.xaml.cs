using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class StartUpPage : ContentPage
{
	public StartUpPage(StartUpViewModel viewModel)
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this,false);
		BindingContext = _viewModel = viewModel;
	}

	private StartUpViewModel _viewModel;

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_viewModel.OnAppearing();
    }
}