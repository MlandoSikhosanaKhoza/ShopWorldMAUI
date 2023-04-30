using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class ProfilePage : ContentPage
{
	private ProfileViewModel _viewModel;
	public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
		MainThread.BeginInvokeOnMainThread(_viewModel.OnAppearingAsync);
    }
}