using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface ICartService
    {
        Task<List<CartModel>> GetCartItemsAsync();
        Task<ObservableCollection<BindCartViewModel>> GetCartViewModelList(ObservableCollection<CartModel> myOrderItems);
        Task<CartModel> GetCartModelByItemId(int ItemId);
        Task<CartModel> AddItemToCartAsync(ItemModel ItemObject);
        Task<CartModel> UpdateCartItem(CartModel CartObject);
        Task<bool> RemoveCartItem(CartModel CartObject);
        Task<bool> SyncPurchases(List<CartModel> Carts);
        Task<int> CountAsync();
    }
}
