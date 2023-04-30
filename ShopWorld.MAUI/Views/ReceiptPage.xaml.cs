using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class ReceiptPage : ContentPage
{
	private ReceiptViewModel _viewModel;
	public ReceiptPage(ReceiptViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=_viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		MainThread.BeginInvokeOnMainThread(_viewModel.OnAppearingAsync);
    }
}