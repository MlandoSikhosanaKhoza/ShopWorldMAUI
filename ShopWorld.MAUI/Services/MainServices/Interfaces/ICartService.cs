using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface ICartService
    {
        Task<List<CartModel>> GetCartItemsAsync();
        Task<CartModel> GetCartModelByItemId(int ItemId);
        Task<CartModel> AddItemToCartAsync(ItemModel ItemObject);
        Task<CartModel> UpdateCartItem(CartModel CartObject);
        Task<bool> RemoveCartItem(CartModel CartObject);
    }
}
