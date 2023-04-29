using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Swagger;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly ShopWorldClient _shopWorldClient;
        public CartService(IUnitOfWork unitOfWork,ShopWorldClient shopWorldClient,IAuthorizationService authorizationService) {
            _cartRepository = unitOfWork.GetRepository<CartModel>();
            _authorizationService = authorizationService;
            _shopWorldClient = shopWorldClient;
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

        public async Task<bool> SyncPurchases(List<CartModel> Carts)
        {
            string customerId = JwtTokenReader.GetTokenValue(_authorizationService.GetToken(), "CustomerId");
            int[] itemId=Carts.Select(c=>c.ItemId).ToArray();
            int[] quantityList=Carts.Select(c=>c.Quantity).ToArray();
            decimal total = Carts.Sum(c => c.Quantity * c.Price);
            try
            {
                Order order = await _shopWorldClient.Order_AddOrderAsync(
                new Order
                {
                    CustomerId = int.Parse(customerId),
                    DateCreated = DateTime.Now,
                    OrderReference = Guid.NewGuid(),
                    Subtotal = total,
                    GrandTotal = (total * Convert.ToDecimal(1.15)),
                });
                await _shopWorldClient.OrderItem_AddOrderItemsAsync(
                    new Shared.OrderItemInputModel
                    {
                        OrderId = order.OrderId,
                        ItemId = itemId,
                        Quantity = quantityList
                    });
                await _cartRepository.DeleteAllAsync();
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
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
