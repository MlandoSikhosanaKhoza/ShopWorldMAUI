using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Swagger;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class UserManagementService:IUserManagementService
    {
        private readonly ShopWorldClient _shopWorldClient;
        private IAuthorizationService _authorizationService;
        private INavigationService _navigationService;
        public UserManagementService(ShopWorldClient shopWorldClient,
            IAuthorizationService authorizationService,
            INavigationService navigationService)
        {
            _shopWorldClient = shopWorldClient;
            _authorizationService = authorizationService;
            _navigationService = navigationService;
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

        public async Task Logout()
        {
            await _authorizationService.WipePersonalDataAsync();
            await _navigationService.NavigateToAsync($"//{nameof(StartUpPage)}");
        }
    }
}
