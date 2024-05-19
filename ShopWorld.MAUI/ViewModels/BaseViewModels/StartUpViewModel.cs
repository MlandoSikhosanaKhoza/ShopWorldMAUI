using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Swagger;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    /// <summary>
    /// This view model governs where the app will navigate on start up
    /// </summary>
    public partial class StartUpViewModel:BaseViewModel
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly INavigationService _navigationService;
        private readonly IItemService _itemService;
        private readonly IOrderItemService _orderItemService;
        private readonly IUserManagementService _userManagementService;
        private readonly ISettingsService _settingsService;
        private ShopWorldClient _shopWorldClient;
        public StartUpViewModel(IAuthorizationService authorizationService, IUserManagementService userManagementService,
            INavigationService navigationService,IItemService itemService,ISettingsService settingsService,
            IOrderItemService orderItemService,ShopWorldClient shopWorldClient) { 
            _authorizationService  = authorizationService;
            _navigationService     = navigationService;
            _itemService           = itemService;
            _orderItemService      = orderItemService;
            _settingsService       = settingsService;
            _userManagementService = userManagementService;
            _shopWorldClient       = shopWorldClient;
        }
        [ObservableProperty]
        private bool mustDisplayLoginButtons = false;



        [RelayCommand]
        private async Task LoginAsUser()
        {
            await _navigationService.NavigateToAsync($"//{nameof(LoginPage)}");
        }

        [RelayCommand]
        private async Task LoginAsAdmin()
        {
            IsBusy = true;
            LoginResult loginResult = await _userManagementService.LoginAsAdmin();
            /* Read JWT Token for Full name*/
            _settingsService.SetFullName(JwtTokenReader.GetTokenValue(loginResult.JwtToken, ClaimTypes.Name));
            await _itemService.CheckAndDownload();
            /* Go to the Shopping Page*/
            IsBusy = false;
            await _navigationService.NavigateToAsync($"//{nameof(ItemPage)}");
            
        }

        public async void OnAppearing()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy                  = true;
            MustDisplayLoginButtons = false;

            /* Check secure storage for JWT Token and store it in a string */
            await _authorizationService.ProcessTokenAsync();
            await Task.Delay(3000);
            if (_authorizationService.IsValidToken())
            {
                _shopWorldClient.AuthorizeClient();
                /*Check if the user is admin or customer */
                string role = JwtTokenReader.GetTokenValue(_authorizationService.GetToken(),ClaimTypes.Role);
                switch (role)
                {
                    case "Customer":
                        IsBusy = false;
                        await _navigationService.NavigateToAsync($"//{nameof(ShoppingPage)}");
                        break;
                    case "Admin":
                        /* Implementation will take place at a later stage */
                        await _navigationService.NavigateToAsync($"//{nameof(ItemPage)}");
                        break;
                }
            }
            else
            {
                MustDisplayLoginButtons = true;
            }
            IsBusy= false;
        }
    }
}
