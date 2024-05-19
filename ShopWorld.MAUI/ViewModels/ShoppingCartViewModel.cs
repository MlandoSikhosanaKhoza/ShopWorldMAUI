using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopWorld.MAUI.Messages;
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
        private readonly INavigationService _navigationService;
        private readonly ICartService _cartService;
        private readonly IDispatcherTimer _updateTimer;
        public ShoppingCartViewModel(
            INavigationService navigationService,
            ICartService cartService,IUpdateCartItemTimedService updateCartItemTimedService) { 
            _navigationService = navigationService;
            _cartService = cartService;
            
            StrongReferenceMessenger.Default.Register<UpdateCartItemMessage>(this, (recipient,message) => {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    updateCartItemTimedService.Execute(message.Value);
                    CalculateTotals();
                });
            });
            StrongReferenceMessenger.Default.Register<DeleteCartItemMessage>(this, (recipient, message) => {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await _cartService.RemoveCartItem(message.Value);
                    BindCartViewModel cartViewModel = MyOrderDisplayItems.Where(cm => cm.ItemId == message.Value.ItemId).FirstOrDefault();
                    MyOrderDisplayItems.Remove(cartViewModel);
                    CartModel cartItemToDelete = MyOrderItems.FirstOrDefault(i => i.ItemId == message.Value.ItemId);
                    if (cartItemToDelete != null)
                    {
                        MyOrderItems.Remove(cartItemToDelete);
                    }
                    CalculateTotals();
                    IsBusy = false;
                });
            });
        }
        private CartModel CartToUpdate { get; set; }
        public ObservableCollection<CartModel> MyOrderItems { get; set; }
        [ObservableProperty]
        private ObservableCollection<BindCartViewModel> myOrderDisplayItems=new ObservableCollection<BindCartViewModel>();
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
            MyOrderItems.Clear();
            MyOrderDisplayItems.Clear();
            StrongReferenceMessenger.Default.Send<ClearCartMessage>(new ClearCartMessage(true));
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
            IsBusy = false;
        }
        
        private void IncreaseQuantity()
        {

        }

        private void CalculateTotals()
        {
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
        }

        private async Task SetupShoppingCart()
        {
            MyOrderDisplayItems = await _cartService.GetCartViewModelList(MyOrderItems);
            TotalBeforeTax = MyOrderItems.Sum(oi => oi.Price * oi.Quantity);
            TotalAfterTax = TotalBeforeTax * (Tax.VAT + 1);
        }

        public async void OnAppearingAsync()
        {
            await SetupShoppingCart();
        }
    }
}
