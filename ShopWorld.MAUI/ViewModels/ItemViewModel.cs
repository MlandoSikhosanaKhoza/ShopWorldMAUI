using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Networking;
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
        private readonly IConnectivity _connectivity;

        public ItemViewModel(IItemService itemService, INavigationService navigationService, IImageService imageService, IConnectivity connectivity)
        {
            _itemService = itemService;
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                Items = await itemService.GetAllBindableItems();
                ExecuteImageDownload();
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
            _connectivity = connectivity;
        }

        [ObservableProperty]
        private ObservableCollection<BindItemViewModel> items = new ObservableCollection<BindItemViewModel>();

        [ObservableProperty]
        private bool isDownloadingImages = false;

        [RelayCommand]
        private async void GoToAdd()
        {
            await _navigationService.NavigateToAsync(nameof(AddItemPage));
        }

        private void ExecuteImageDownload()
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                IsDownloadingImages = true;
                foreach (BindItemViewModel item in Items)
                {
                    if (!item.ImageIsDownloaded)
                    {
                        if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            item.DownloadInProgress = true;
                            for (int i = 0; i < 3; i++)
                            {
                                //String is imageName and value is if it downloaded
                                KeyValuePair<string, bool> imageDownload = await _itemService.DownloadImageForItemAsync(item.GetItemModel());
                                bool isDownloaded = imageDownload.Value;

                                if (isDownloaded)
                                {
                                    item.ImageName = imageDownload.Key;
                                    item.ImageDisplaySource = imageDownload.Key;
                                    break;
                                }
                                else
                                {
                                    item.IsFailedDownload = true;
                                }
                            }
                            item.DownloadInProgress = false;
                        }
                        else
                        {
                            item.IsFailedDownload = true;
                        }
                    }
                    await Task.Delay(200);
                }
                IsDownloadingImages = false;
            });
        }

        public async Task OnAppearing()
        {
            if (Refresh)
            {
                Items = await _itemService.GetAllBindableItems();
                ExecuteImageDownload();
                Refresh = false;
            }
        }
    }
}
