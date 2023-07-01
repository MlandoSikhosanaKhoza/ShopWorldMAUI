﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopWorld.MAUI.Messages;
using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class ShoppingViewModel : BaseViewModel
    {
        private IItemService _itemService;
        private ICartService _cartService;
        private INavigationService _navigationService;
        public ShoppingViewModel(IItemService itemService,
            ICartService cartService,
            INavigationService navigationService)
        {
            _itemService = itemService;
            _cartService = cartService;
            _navigationService = navigationService;
            StrongReferenceMessenger.Default.Register<UpdateCartItemMessage>(this, (recipient, message) => {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CartModel item = message.Value;
                    MyOrderItems.FirstOrDefault(i=>i.CartId==item.CartId).Quantity = item.Quantity;
                    OnPropertyChanged(nameof(NumOfWantedItems));
                });
            });
            StrongReferenceMessenger.Default.Register<DeleteCartItemMessage>(this, (recipient, message) => {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CartModel cartItemToDelete=MyOrderItems.FirstOrDefault(i=>i.ItemId == message.Value.ItemId);
                    if (cartItemToDelete != null)
                    {
                        MyOrderItems.Remove(cartItemToDelete);
                    }
                });
            });
            StrongReferenceMessenger.Default.Register<ClearCartMessage>(this, (recipient, message) => {
                MyOrderItems.Clear();
            });
            StrongReferenceMessenger.Default.Register<LogoutMessage>(this, (recipient, message) => {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MyOrderItems = new ObservableCollection<CartModel>();
                    HasAccessedPage = false;
                });
            });
        }
        [ObservableProperty]
        private ObservableCollection<ItemModel> items;
        [ObservableProperty]
        private ObservableCollection<CartModel> myOrderItems = new ObservableCollection<CartModel>();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoadingItems))]
        private bool isLoadingItems = false;
        [ObservableProperty]
        private bool hasAccessedPage = false;
        public bool IsNotLoadingItems => !IsLoadingItems;
        public int NumOfWantedItems => MyOrderItems.Sum(oi => oi.Quantity);

        [RelayCommand]
        private async void RefreshItems()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            IsLoadingItems = true;
            await _itemService.ReSynchronizeItemsAsync();
            Items=new ObservableCollection<ItemModel>(await _itemService.GetAllItemsAsync());
            IsLoadingItems=false;
            IsBusy = false;
        }

        [RelayCommand]
        private async void CompleteOrder()
        {
            Dictionary<string,object> data = new Dictionary<string,object>();
            data.Add("MyOrderItems", MyOrderItems);
            await _navigationService.NavigateToAsync($"{nameof(ShoppingCartPage)}",data);
        }

        [RelayCommand]
        private async void PurchaseItem(ItemModel ItemObject)
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            CartModel addedCartItem = await _cartService.AddItemToCartAsync(ItemObject);
            if (MyOrderItems.Any(c => c.ItemId == addedCartItem.ItemId))
            {
                CartModel cartModelToUpdate = MyOrderItems.FirstOrDefault(c => c.ItemId == ItemObject.ItemId);
                cartModelToUpdate.Quantity++;
            }
            else
            {
                //Theres an internal bug with the persistence of the OrderItems
                //This is a temporary fix (I'll find a better fix for this)
                addedCartItem.Quantity = 1;
                await _cartService.UpdateCartItem(addedCartItem);
                MyOrderItems.Add(addedCartItem);
            }
            OnPropertyChanged(nameof(NumOfWantedItems));
            IsBusy = false;
        }

        public async void OnAppearingAsync()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            if(Items==null) {
                Items = new ObservableCollection<ItemModel>(await _itemService.GetAllItemsAsync());
            }
            if((await _cartService.CountAsync())>0  && !HasAccessedPage)
            {
                MyOrderItems = new ObservableCollection<CartModel>(await _cartService.GetCartItemsAsync());
            }
            OnPropertyChanged(nameof(NumOfWantedItems));
            IsBusy = false;
            HasAccessedPage = true;
        }
    }
}
