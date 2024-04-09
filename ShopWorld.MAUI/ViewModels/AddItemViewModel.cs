using Camera.MAUI.ZXingHelper;
using Camera.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Validation;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using static SQLite.SQLite3;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class AddItemViewModel:BaseViewModel
    {
        [ObservableProperty]
        private ImageSource imageToDisplay;
        [ObservableProperty]
        private ValidatableObject<string> description=new ValidatableObject<string>();
        [ObservableProperty]
        private ValidatableObject<string> price=new ValidatableObject<string>();
        [ObservableProperty]
        private bool isActivePage=false;
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
        private readonly IItemService _itemService;
        private readonly INavigationService _navigationService;
        private readonly IImageService _imageService;
        private CameraView cameraView { get; set; }
        public AddItemViewModel(IItemService itemService,INavigationService navigationService,IImageService imageService)
        {
            _itemService = itemService;
            _navigationService = navigationService;
            _imageService = imageService;
            Description.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage="Required *" });
            Price.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage="Required *" });
            Price.Validations.Add(new StringPriceValid<string> { ValidationMessage="Price should be 2 decimal places" });
            Description.Value = "";
            Price.Value = "";
        }

        public void SetCameraView(CameraView CameraViewObject)
        {
            this.cameraView = CameraViewObject;
            this.cameraView.Camera = this.cameraView.Cameras.First();
            this.cameraView.ForceAutoFocus();
        }

        [RelayCommand]
        private void ForceAutoFocus()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            if(IsActivePage)
            {
                cameraView.ForceAutoFocus();
            }
            IsBusy= false;
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
                    
                    SKImage image = SKImage.FromEncodedData(stream);
                    int maxDimension = 300;
                    double newHeight = image.Height >= image.Width ? maxDimension : (((double)image.Height / (double)image.Width) * maxDimension);
                    double newWidth = image.Width >= image.Height ? maxDimension : (((double)image.Width / (double)image.Height) * maxDimension);
                    SKBitmap bitmap = SKBitmap.FromImage(image);
                    SKBitmap nbitmap = bitmap.Resize(new SKImageInfo((int)newWidth,(int)newHeight), SKFilterQuality.High);
                    image = SKImage.FromBitmap(nbitmap);
                    SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
                    imageToUpload = data.ToArray();
                    stream = new MemoryStream(imageToUpload);
                    SnapShotSource = ImageSource.FromStream(() => stream);
                    await this.cameraView.StopCameraAsync();

                }
                else
                {
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
        private async void AddItem()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            bool isValidDescription = Description.Validate();
            bool isValidPrice = Price.Validate();
            if(isValidDescription && isValidPrice)
            {
                if (imageToUpload != null)
                {
                    byte[] smallImage = imageToUpload;
                    Item item = await _itemService.AddItemAsync(new Shared.ItemInputModel
                    {
                        ItemId = 0,
                        ImageName = $"{Guid.NewGuid()}.png",
                        Base64 = Convert.ToBase64String(smallImage),
                        Description = Description.Value,
                        Price = decimal.Parse(Price.Value)
                    });
                    if (item != null)
                    {
                        await _navigationService.NavigateToAsync($"//{nameof(ItemPage)}?Refresh=true");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Image required", "Please take a photo before proceeding.", "OK");
                }
                
            }
            IsBusy= false;
        }

        public async void OnAppearing()
        {
        }
    }
}
