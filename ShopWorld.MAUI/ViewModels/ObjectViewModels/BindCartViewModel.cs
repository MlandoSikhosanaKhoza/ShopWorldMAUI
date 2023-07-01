using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopWorld.MAUI.Messages;
using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class BindCartViewModel:ObservableObject
    {
        private readonly CartModel _item;
        public BindCartViewModel(CartModel item)
        {
            _item = item;
        }

        public int CartId
        {
            get { return _item.CartId; }
            set { _item.CartId = value; OnPropertyChanged(nameof(CartId)); }
        }

        public int ItemId {
            get { return _item.ItemId; }
            set { _item.ItemId = value; OnPropertyChanged(nameof(ItemId)); }
        }
        
        public string ItemName
        {
            get { return _item.ItemName; }
            set { _item.ItemName = value; OnPropertyChanged(nameof(ItemName)); } 
        }

        public int Quantity
        {
            get { return _item.Quantity; }
            set { _item.Quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }

        public decimal Price {
            get { return _item.Price; }
            set { _item.Price = value; OnPropertyChanged(nameof(Price)); }
        }
        public DateTime OrderDate
        {
            get => _item.OrderDate;
            set { _item.OrderDate = value; OnPropertyChanged(nameof(OrderDate)); }
        }

        [RelayCommand]
        private void IncreaseQuantity()
        
        {
            Quantity++;
            StrongReferenceMessenger.Default.Send(new UpdateCartItemMessage(_item));
        }

        [RelayCommand]
        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
                StrongReferenceMessenger.Default.Send(new UpdateCartItemMessage(_item));
            }
        }

        [RelayCommand]
        private void DeleteCartItem()
        {
            StrongReferenceMessenger.Default.Send(new DeleteCartItemMessage(_item));
        }
    }
}
