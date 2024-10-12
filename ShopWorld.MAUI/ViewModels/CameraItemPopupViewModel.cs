using Camera.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views.Modal;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class CameraItemPopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ImageSource imageToDisplay;
        [ObservableProperty]
        private bool isActivePage = false;
        [ObservableProperty]
        private ImageSource snapShotSource;
        #region Image Toggler
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsVisibleSnapShot))]
        private bool isVisibleCameraView = true;
        public bool IsVisibleSnapShot
        {
            get { return !IsVisibleCameraView; }
        }

        #endregion ImageToggler
        private byte[] imageToUpload { get; set; } = null;
        private CameraView cameraView { get; set; }
        private CameraItemPopup popup { get; set; }

        public void SetCameraView(CameraView CameraViewObject)
        {
            this.cameraView = CameraViewObject;
            this.cameraView.Camera = this.cameraView.Cameras.First();
            this.cameraView.ForceAutoFocus();
        }

        public void SetPopup(CameraItemPopup Popup)
        {
            popup = Popup;
        }

        [RelayCommand]
        private void ForceAutoFocus()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            if (IsActivePage)
            {
                cameraView.ForceAutoFocus();
            }
            IsBusy = false;
        }

        [RelayCommand]
        private async void TakePhoto()
        {
            if (IsBusy)
            {
                return;
            }
            if (IsActivePage)
            {
                IsBusy = true;
                if (IsVisibleCameraView)
                {

                    MemoryStream stream = (MemoryStream)await this.cameraView.TakePhotoAsync();

                    SKImage image    = SKImage.FromEncodedData(stream);
                    int maxDimension = 300;
                    double newHeight = image.Height >= image.Width ? maxDimension : (((double)image.Height / (double)image.Width) * maxDimension);
                    double newWidth  = image.Width >= image.Height ? maxDimension : (((double)image.Width / (double)image.Height) * maxDimension);
                    SKBitmap bitmap  = SKBitmap.FromImage(image);
                    SKBitmap nbitmap = bitmap.Resize(new SKImageInfo((int)newWidth, (int)newHeight), SKFilterQuality.High);
                    image            = SKImage.FromBitmap(nbitmap);
                    SKData data      = image.Encode(SKEncodedImageFormat.Png, 100);
                    imageToUpload    = data.ToArray();
                    stream           = new MemoryStream(imageToUpload);
                    SnapShotSource   = ImageSource.FromStream(() => stream);
                    await this.cameraView.StopCameraAsync();

                }
                else
                {
                    await Task.Delay(2000);
                    await this.cameraView.StartCameraAsync();
                }
                IsBusy = false;
                IsVisibleCameraView = !IsVisibleCameraView;
            }

        }

        [RelayCommand]
        private async void StartCamera()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
            IsBusy = false;
        }

        [RelayCommand]
        private async Task Continue()
        {
            await popup.CloseAsync(imageToUpload);
        }

        public async void OnAppearing()
        {
        }
    }
}
