using CommunityToolkit.Mvvm.ComponentModel;
using ShopWorld.MAUI.Models;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class ShoppingViewModel:BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ItemModel> items;


        public async void OnAppearingAsync()
        {

        }
    }
}
