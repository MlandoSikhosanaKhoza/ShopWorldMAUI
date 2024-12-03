using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    public partial class ReceiptDetailViewModel:BaseViewModel
    {
        public int OrderId { get; set; }

        private IOrderService _orderService;
        private IOrderItemService _orderItemService;
        private INavigationService _navigationService;

        public ReceiptDetailViewModel(IOrderService orderService,IOrderItemService orderItemService,INavigationService navigationService) { 
            _orderItemService = orderItemService;
            _orderService = orderService;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private OrderModel orderModel;
        [ObservableProperty]
        private ObservableCollection<OrderItemModel> orderItems;

        [RelayCommand]
        private async void GoBack()
        {
            await _navigationService.NavigateToAsync($"//{nameof(ReceiptPage)}");
        }

        public async void OnAppearingAsync()
        {
            OrderModel = await _orderService.GetOrderModelByIdAsync(OrderId);
            OrderItems= new ObservableCollection<OrderItemModel>(await _orderItemService.GetOrderItemsByOrderAsync(OrderId));
        }
    }
}
