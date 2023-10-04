
using Camera.MAUI;
using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views;

public partial class AddItemPage : ContentPage
{
	private AddItemViewModel _viewModel;
	public AddItemPage(AddItemViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    protected override void OnAppearing()
    {
		_viewModel.OnAppearing();
    }

    private void CameraDisplay_CamerasLoaded(object sender, EventArgs e)
    {
        if (CameraDisplay.NumCamerasDetected > 0)
        {
            if (CameraDisplay.NumMicrophonesDetected > 0)
                CameraDisplay.Microphone = CameraDisplay.Microphones.First();
            CameraDisplay.Camera = CameraDisplay.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (await CameraDisplay.StartCameraAsync() == CameraResult.Success)
                {
                    _viewModel.IsActivePage = true;
                    _viewModel.SetCameraView(CameraDisplay);
                }
            });
        }
    }
}