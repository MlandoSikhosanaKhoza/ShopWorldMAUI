using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class ItemPage : ContentPage
{
	private readonly ItemViewModel _viewModel;
	public ItemPage(ItemViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
		_viewModel=viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		MainThread.BeginInvokeOnMainThread(async () => { 
			await _viewModel.OnAppearing();
		});
    }
}