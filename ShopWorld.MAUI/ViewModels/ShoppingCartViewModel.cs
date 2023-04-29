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
        private decimal totalBeforeTax;

        [RelayCommand]
        private async void GoBack()
        {
            await _navigationService.NavigateToAsync($"//{nameof(ShoppingPage)}");
        }

        [RelayCommand]
        private async void IncreaseQuantity(CartModel cartModel)
        {
            if(IsBusy) return;
            IsBusy = true;
            cartModel.Quantity++;
            await _cartService.UpdateCartItem(cartModel);
            MyOrderItems = new ObservableCollection<CartModel>(await _cartService.GetCartItemsAsync());
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }

        [RelayCommand]
        private async void DecreaseQuantity(CartModel cartModel)
        {
            if (IsBusy) return;
            IsBusy = true;
            cartModel.Quantity--;
            if (cartModel.Quantity==0)
            {
                await _cartService.RemoveCartItem(cartModel);
            }
            else
            {
                await _cartService.UpdateCartItem(cartModel);
            }
            MyOrderItems = new ObservableCollection<CartModel>(await _cartService.GetCartItemsAsync());
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }

        [RelayCommand]
        private async void RemoveCartItem(CartModel cartModel)
        {
            if (IsBusy) return;
            IsBusy = true;
            await _cartService.RemoveCartItem(cartModel);
            MyOrderItems = new ObservableCollection<CartModel>(await _cartService.GetCartItemsAsync());
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }

        [ObservableProperty]
        private decimal totalAfterTax;
        public async void OnAppearingAsync() { 
            MyOrderItems = new ObservableCollection<CartModel>(await _cartService.GetCartItemsAsync());
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT+1);
        }
    }
}
