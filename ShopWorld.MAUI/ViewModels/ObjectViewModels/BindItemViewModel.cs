using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopWorld.MAUI.Messages;
using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Skia;
using ShopWorld.MAUI.Validation;
using ShopWorld.MAUI.Views.Modal;
using ShopWorld.Shared.Entities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class BindItemViewModel:ObservableObject
    {
        public BindItemViewModel(ItemModel item) { 
            _item = item;
            price = item.Price.ToString("0.00");
            ImageDisplaySource = item.ImageName;
            DescriptionCheck.Validations.Add(new IsNotNullOrEmptyRule<string>() { ValidationMessage = "Required * "});
            PriceCheck.Validations.Add(new StringPriceValid<string>() { ValidationMessage="Must be number of 2 decimal places"});
        }
        private ItemModel _item;
        private byte[] imageToUpload { get; set; } = null;


        public ItemModel GetItemModel()
        {
            return _item;
        }
        [ObservableProperty]
        private ImageSource imageDisplaySource;

        public int ItemId {
            get { return _item.ItemId; }
            set { _item.ItemId = value; OnPropertyChanged(nameof(ItemId)); }
        }
        
        public string ImageName {
            get { return _item.ImageName; }
            set { _item.ImageName = value; OnPropertyChanged(nameof(ImageName)); }
        }
        
        public string Description { 
            get { return _item.Description; } 
            set { _item.Description = value;DescriptionCheck.Value = value; OnPropertyChanged(nameof(Description)); } 
        }
        [ObservableProperty]
        private ValidatableObject<string> descriptionCheck = new ValidatableObject<string>();
        private string price;
        public string Price
        {
            get { return price; }
            set {
                price = value;
                PriceCheck.Value = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        [ObservableProperty]
        private ValidatableObject<string> priceCheck = new ValidatableObject<string>();

        public bool ImageIsDownloaded { get => !ImageName.EndsWith("image_not_found.png"); }

        [ObservableProperty]
        private bool isFailedDownload = false;

        [ObservableProperty]
        private bool downloadInProgress = false;

        [RelayCommand]
        private async Task TakePhoto()
        {
            try
            {
                CameraItemPopup popup = new CameraItemPopup(new CameraItemPopupViewModel());
                object? imageResult = await Shell.Current.ShowPopupAsync(popup);
                
                if(imageResult is byte[] imageByte)
                {
                    imageToUpload       = imageByte;
                    MemoryStream stream = new MemoryStream(imageToUpload);
                    ImageDisplaySource  = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception e)
            {
                
            }
            
        }

        [RelayCommand]
        private void Save()
        {
            bool isValidDescription = DescriptionCheck.Validate();
            bool isValidPriceCheck = PriceCheck.Validate();
            if (isValidDescription && isValidPriceCheck)
            {
                _item.Description = Description;
                _item.Price=decimal.Parse(Price);

                StrongReferenceMessenger.Default.Send<SaveItemMessage>(new SaveItemMessage(
                    new Shared.ItemInputModel { 
                        ItemId      = _item.ItemId,
                        ImageName   = _item.ImageName,
                        Base64      = imageToUpload == null? null : Convert.ToBase64String(imageToUpload),
                        Description = _item.Description,
                        Price       = _item.Price,
                        IsDeleted   = false 
                    })); 
            }
        }
    }
}
