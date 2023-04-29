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
    public partial class ReceiptViewModel:BaseViewModel
    {
        private INavigationService _navigationService;
        private IOrderService _orderService;
        public ReceiptViewModel(IOrderService orderService,INavigationService navigationService) { 
            _orderService = orderService;
            _navigationService= navigationService;
        }
        [ObservableProperty]
        private ObservableCollection<OrderModel> orders;

        [RelayCommand]
        private async void GoToReceipt(OrderModel OrderModelObject)
        {
            await _navigationService
                .NavigateToAsync(
                $"{nameof(ReceiptDetailPage)}?{nameof(ReceiptDetailViewModel.OrderId)}={OrderModelObject.OrderId}"
                );
        }
        public async void OnAppearingAsync()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy= true;

            Orders=new ObservableCollection<OrderModel>(await _orderService.GetAllOrdersAsync());


            IsBusy= false;
        }
    }
}
