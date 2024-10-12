using Camera.MAUI;
using CommunityToolkit.Maui.Views;
using ShopWorld.MAUI.ViewModels;

namespace ShopWorld.MAUI.Views.Modal;

public partial class CameraItemPopup : Popup
{
	private readonly CameraItemPopupViewModel _viewModel;
    public CameraItemPopup(CameraItemPopupViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.SetPopup(this);
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