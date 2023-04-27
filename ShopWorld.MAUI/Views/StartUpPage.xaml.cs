using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class StartUpPage : ContentPage
{
	public StartUpPage(StartUpViewModel viewModel)
	{
        BindingContext = _viewModel = viewModel;
        InitializeComponent();
		Shell.SetNavBarIsVisible(this,false);
		
	}

	private StartUpViewModel _viewModel;

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_viewModel.OnAppearing();
    }
}