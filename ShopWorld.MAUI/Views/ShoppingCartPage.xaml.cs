using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class ShoppingCartPage : ContentPage
{

    private ShoppingCartViewModel _viewModel;
    public ShoppingCartPage(ShoppingCartViewModel viewModel)
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = _viewModel = viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearingAsync();
    }
}