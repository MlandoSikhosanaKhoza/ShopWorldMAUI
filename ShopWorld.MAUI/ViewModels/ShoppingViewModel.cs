using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        }
        [ObservableProperty]
        private ObservableCollection<ItemModel> items;
        [ObservableProperty]
        private ObservableCollection<CartModel> myOrderItems = new ObservableCollection<CartModel>();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoadingItems))]
        private bool isLoadingItems = false;

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
                MyOrderItems = new ObservableCollection<CartModel>(await _cartService.GetCartItemsAsync());
            }
            OnPropertyChanged(nameof(NumOfWantedItems));
            IsBusy = false;
        }
    }
}
