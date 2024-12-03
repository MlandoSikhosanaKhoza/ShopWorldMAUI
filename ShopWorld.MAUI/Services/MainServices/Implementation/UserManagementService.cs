using ShopWorld.MAUI.Models;
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
    public class UserManagementService : IUserManagementService
    {
        private readonly ShopWorldClient _shopWorldClient;
        private IAuthorizationService _authorizationService;
        private INavigationService _navigationService;
        private IConnectivity _connectivity;
        public UserManagementService(ShopWorldClient shopWorldClient,
            IAuthorizationService authorizationService,
            INavigationService navigationService,IConnectivity connectivity)
        {
            _shopWorldClient      = shopWorldClient;
            _authorizationService = authorizationService;
            _navigationService    = navigationService;
            _connectivity         = connectivity;
        }

        public async Task<LoginResult> LoginAsUser(string Mobile)
        {
            LoginResult login = new LoginResult();
            if(_connectivity.NetworkAccess==NetworkAccess.Internet)
            {
                try
                {
                    login = await _shopWorldClient.Authorization_LoginAsync(new MobileLoginInputModel { MobileNumber = Mobile });
                    if (login.IsAuthorized)
                    {
                        /* Store the JWT Token */
                        await _authorizationService.SetLoginToken(login.JwtToken);

                        _shopWorldClient.AuthorizeClient();
                    }
                }
                catch (ApiException ex)
                {
                    login.IsAuthorized = false;
                }
            }
            else
            {
                login.IsAuthorized=false;
            }
            
            return login;
        }

        public async Task<CustomerModel> Register(CustomerModel CustomerObj)
        {
            CustomerModel customer = await _shopWorldClient.Customer_AddCustomerAsync(CustomerObj);
            return customer;
        }

        public async Task Logout()
        {
            await _authorizationService.WipePersonalDataAsync();
            await _navigationService.NavigateToAsync($"//{nameof(StartUpPage)}");
        }

        public async Task<bool> MobileNumberExists(string Mobile)
        {
            bool mobileExists=await _shopWorldClient.Customer_MobileNumberExistsAsync(Mobile);
            return mobileExists;
        }

        public async Task<LoginResult> LoginAsAdmin()
        {
            LoginResult loginResult=new LoginResult();
            if (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                loginResult = await _shopWorldClient.Authorization_LoginAsAdminAsync();
                await _authorizationService.SetLoginToken(loginResult.JwtToken);
                _shopWorldClient.AuthorizeClient();
            }
            else
            {
                loginResult.IsAuthorized = false;
            }
            return loginResult;
        }
    }
}
