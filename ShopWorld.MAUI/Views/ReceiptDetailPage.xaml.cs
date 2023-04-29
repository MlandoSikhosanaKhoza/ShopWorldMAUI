using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class ReceiptDetailPage : ContentPage
{
	private readonly ReceiptDetailViewModel _viewModel;
	public ReceiptDetailPage(ReceiptDetailViewModel viewModel)
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
		BindingContext=_viewModel=viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		MainThread.BeginInvokeOnMainThread(_viewModel.OnAppearingAsync);
    }
}