using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class ItemPage : ContentPage
{
	public ItemPage(ItemViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}