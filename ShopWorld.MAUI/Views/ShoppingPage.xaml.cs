using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class ShoppingPage : ContentPage
{
	private ShoppingViewModel _viewModel;
	public ShoppingPage(ShoppingViewModel viewModel)
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
		Shell.SetTabBarIsVisible(this, true);
		BindingContext=_viewModel=viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		MainThread.BeginInvokeOnMainThread(_viewModel.OnAppearingAsync);
    }
}