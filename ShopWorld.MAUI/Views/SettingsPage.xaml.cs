using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}