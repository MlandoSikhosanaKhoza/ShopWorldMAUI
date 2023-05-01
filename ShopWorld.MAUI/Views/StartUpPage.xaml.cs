using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class StartUpPage : ContentPage
{
    private StartUpViewModel _viewModel;
    public StartUpPage(StartUpViewModel viewModel)
	{
        BindingContext = _viewModel = viewModel;
        InitializeComponent();
		Shell.SetNavBarIsVisible(this,false);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_viewModel.OnAppearing();
    }
}