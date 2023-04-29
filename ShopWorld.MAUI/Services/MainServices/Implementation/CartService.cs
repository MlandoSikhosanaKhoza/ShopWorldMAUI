using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    /// <summary>
    /// This service allows the user to save and manage their purchases they make both online and offline
    /// </summary>
    public class CartService:ICartService
    {
        private readonly IGenericRepository<CartModel> _cartRepository;
        public CartService(IUnitOfWork unitOfWork) {
            _cartRepository = unitOfWork.GetRepository<CartModel>();
        }

        public async Task<CartModel> GetCartModelByItemId(int ItemId)
        {
            CartModel cartModel=(await _cartRepository.GetAsync(c=>c.ItemId==ItemId)).FirstOrDefault();
            return cartModel;
        }

        public async Task<List<CartModel>> GetCartItemsAsync()
        {
            return await _cartRepository.GetAsync();
        }

        public async Task<CartModel> AddItemToCartAsync(ItemModel ItemObject)
        {
            CartModel cart = await GetCartModelByItemId(ItemObject.ItemId);
            if (cart == null)
            {
                cart = new CartModel
                {
                    CartId = 0,
                    ItemId = ItemObject.ItemId,
                    ItemName = ItemObject.Description,
                    Quantity = 1,
                    Price = ItemObject.Price,
                    OrderDate = DateTime.Now
                };
                int CartId=await _cartRepository.InsertAsync(cart);
                cart.CartId = CartId;
            }
            else
            {
                cart.Quantity = cart.Quantity + 1;
                await _cartRepository.UpdateAsync(cart);
            }
            return cart;
        }

        public async Task<CartModel> UpdateCartItem(CartModel CartObject)
        {
            await _cartRepository.UpdateAsync(CartObject);
            return CartObject;
        }

        public async Task<bool> RemoveCartItem(CartModel CartObject)
        {
            await _cartRepository.DeleteAsync(CartObject);
            return true;
        }
    }
}
