using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopWorld.MAUI.Messages;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    [QueryProperty(nameof(Refresh),nameof(Refresh))]
    public partial class ItemViewModel:BaseViewModel
    {
        public bool Refresh { get; set; } = false;

        private readonly IItemService _itemService;
        private readonly IImageService _imageService;
        private readonly INavigationService _navigationService;

        public ItemViewModel(IItemService itemService, INavigationService navigationService, IImageService imageService)
        {
            _itemService = itemService;
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                Items = await itemService.GetAllBindableItems();
            });

            StrongReferenceMessenger.Default.Register<SaveItemMessage>(this, async (recipient, message) =>
            {
                if (message.Value.ItemId == 0)
                {
                    Item itemAdded = await itemService.AddItemAsync(message.Value);
                    if (itemAdded != null)
                    {
                        Items.Add(new BindItemViewModel(new
                            Models.ItemModel
                        {
                            ItemId = message.Value.ItemId,
                            Description = message.Value.Description,
                            Price = message.Value.Price,
                            DateSynced = DateTime.Now,
                            IsDeleted = message.Value.IsDeleted
                        }
                        ));
                        await Shell.Current.DisplayAlert("Saved!", "Item has been successfully added!", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Failed!", "Item has failed to add!", "OK");
                    }
                }
                else
                {
                    byte[] smallImage=imageService.DownSizeImage(Convert.FromBase64String(message.Value.Base64));
                    message.Value.Base64=Convert.ToBase64String(smallImage);
                    bool isUpdated = await itemService.UpdateItemAsync(message.Value);
                    if (isUpdated)
                    {
                        await Shell.Current.DisplayAlert("Saved!", "Item has been successfully updated!", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Failed!", "Item has failed to update!", "OK");
                    }
                }
            });
            _navigationService = navigationService;
            _imageService = imageService;
        }

        [ObservableProperty]
        private ObservableCollection<BindItemViewModel> items = new ObservableCollection<BindItemViewModel>();

        [RelayCommand]
        private async void GoToAdd()
        {
            await _navigationService.NavigateToAsync(nameof(AddItemPage));
        }

        public async Task OnAppearing()
        {
            if (Refresh)
            {
                Items = await _itemService.GetAllBindableItems();
                Refresh = false;
            }
        }
    }
}
