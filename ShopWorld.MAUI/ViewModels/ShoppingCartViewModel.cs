using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    [QueryProperty(nameof(MyOrderItems),nameof(MyOrderItems))]
    public partial class ShoppingCartViewModel:BaseViewModel
    {
        private INavigationService _navigationService;
        private ShoppingViewModel _shoppingViewModel;
        private ICartService _cartService;
        public ShoppingCartViewModel(ShoppingViewModel shoppingViewModel,
            INavigationService navigationService,
            ICartService cartService) { 
            _navigationService = navigationService;
            _cartService = cartService;
            _shoppingViewModel  = shoppingViewModel;
        }
        [ObservableProperty]
        private ObservableCollection<CartModel> myOrderItems=new ObservableCollection<CartModel>();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsDisplayingCheckoutButton))]
        private decimal totalBeforeTax;
        [ObservableProperty]
        private decimal totalAfterTax;

        public bool IsDisplayingCheckoutButton => TotalBeforeTax > 0;
        [RelayCommand]
        private async void GoBack()
        {
            await _navigationService.NavigateToAsync($"//{nameof(ShoppingPage)}");
        }

        [RelayCommand]
        private async void Checkout()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            /* I might actually tell the user that the purchase was made successfully */
            bool isSyncedPurchases=await _cartService.SyncPurchases(MyOrderItems.ToList());
            MyOrderItems = new ObservableCollection<CartModel>(await _cartService.GetCartItemsAsync());
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }

        [RelayCommand]
        private void IncreaseQuantity(CartModel cartModel)
        {
            if(IsBusy) return;
            IsBusy = true;
            cartModel.Quantity++;
            MainThread.BeginInvokeOnMainThread(async()=>{
                await _cartService.UpdateCartItem(cartModel);
            });
            MyOrderItems = new ObservableCollection<CartModel>(MyOrderItems);
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }

        [RelayCommand]
        private void DecreaseQuantity(CartModel cartModel)
        {
            if (IsBusy) return;
            IsBusy = true;
            cartModel.Quantity--;
            if (cartModel.Quantity==0)
            {
                MainThread.BeginInvokeOnMainThread(async () => {
                    await _cartService.RemoveCartItem(cartModel);
                });
                
                MyOrderItems.Remove(cartModel);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>{
                    await _cartService.UpdateCartItem(cartModel);
                });
            }
            MyOrderItems = new ObservableCollection<CartModel>(MyOrderItems);
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }

        [RelayCommand]
        private void RemoveCartItem(CartModel cartModel)
        {
            if (IsBusy) return;
            IsBusy = true;
            MainThread.BeginInvokeOnMainThread(async () => {
                await _cartService.RemoveCartItem(cartModel);
            });
            MyOrderItems.Remove(cartModel);
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }

        
        public async void OnAppearing() {
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT+1);
        }
    }
}
