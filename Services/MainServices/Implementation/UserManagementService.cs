using ShopWorld.MAUI.Swagger;
using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class UserManagementService
    {
        private readonly ShopWorldClient _shopWorldClient;
        private readonly IAuthorizationService _authorizationService;
        public UserManagementService(ShopWorldClient shopWorldClient, IAuthorizationService authorizationService)
        {
            _shopWorldClient = shopWorldClient;
            _authorizationService = authorizationService;
        }

        public async Task<LoginResult> LoginAsUser(string Mobile)
        {
            LoginResult login = new LoginResult();
            try
            {
                login = await _shopWorldClient.Authorization_LoginAsync(new MobileLoginInputModel { MobileNumber = Mobile });
            }
            catch (ApiException ex)
            {
                login.IsAuthorized = false;
            }
            return login;
        }
    }
}
