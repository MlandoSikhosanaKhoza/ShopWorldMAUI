using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Repository;
using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class AuthorizationService:IAuthorizationService
    {
        /* Initialize: Wipe data for user shopping cart */
        private IGenericRepository<CartModel> _cartRepository;
        private IGenericRepository<OrderModel> _orderRepository;
        private IGenericRepository<OrderItemModel> _orderItemRepository;
        /* Initialize: Wipe customer data from admin */
        private IGenericRepository<CustomerModel> _customerRepository;
        public AuthorizationService(IUnitOfWork unitOfWork) { 
            _cartRepository = unitOfWork.GetRepository<CartModel>();
            _customerRepository = unitOfWork.GetRepository<CustomerModel>();
            _orderRepository = unitOfWork.GetRepository<OrderModel>();
            _orderItemRepository = unitOfWork.GetRepository<OrderItemModel>();
        }
        private string Token { get; set; }
        public async Task SetLoginToken(string Token)
        {
            this.Token = Token;
            await SecureStorage.SetAsync("login_token", Token);
        }

        public string GetToken()
        {
            return this.Token;
        }

        public async Task ProcessTokenAsync()
        {
            Token = await SecureStorage.GetAsync("login_token");
        }

        public async Task WipePersonalDataAsync()
        {
            SecureStorage.RemoveAll();
            Preferences.Remove("Username");
            await _cartRepository.DeleteAllAsync();
            await _customerRepository.DeleteAllAsync();
            await _orderRepository.DeleteAllAsync();
            await _orderItemRepository.DeleteAllAsync();
        }

        public bool IsValidToken()
        {
            if (!string.IsNullOrEmpty(this.Token))
            {
                JwtSecurityToken jwtSecurityToken =JwtTokenReader.GetJwtToken(this.Token);
                if (DateTime.UtcNow < jwtSecurityToken.ValidTo)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
