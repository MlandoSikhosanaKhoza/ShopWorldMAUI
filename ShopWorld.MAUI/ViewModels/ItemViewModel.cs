using CommunityToolkit.Mvvm.ComponentModel;
using ShopWorld.MAUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class ItemViewModel:BaseViewModel
    {
        private readonly IItemService _itemService;
        public ItemViewModel(IItemService itemService) {
            _itemService = itemService;
            MainThread.InvokeOnMainThreadAsync(async () => { 
                Items=await itemService.GetAllBindableItems();
            });
        }
        
        [ObservableProperty]
        private ObservableCollection<BindItemViewModel> items = new ObservableCollection<BindItemViewModel>();

        
    }
}
