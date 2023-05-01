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
        private IAuthorizationService _authorizationService;
        private INavigationService _navigationService;
        private IItemService _itemService;
        private IOrderItemService _orderItemService;
        private ShopWorldClient _shopWorldClient;
        public StartUpViewModel(IAuthorizationService authorizationService,
            INavigationService navigationService,IItemService itemService,
            IOrderItemService orderItemService,ShopWorldClient shopWorldClient) { 
            _authorizationService = authorizationService;
            _navigationService = navigationService;
            _itemService = itemService;
            _orderItemService = orderItemService;
            _shopWorldClient = shopWorldClient;
        }
        [ObservableProperty]
        private bool mustDisplayLoginButtons=false;

        [RelayCommand]
        private async void LoginAsUser()
        {
            await _navigationService.NavigateToAsync($"//{nameof(LoginPage)}");
        }

        [RelayCommand]
        private void LoginAsAdmin()
        {

        }

        public async void OnAppearing()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            /* Download the items I can purchase */
            await _itemService.CheckAndDownload();

            /* Check secure storage for JWT Token and store it in a string */
            await _authorizationService.ProcessTokenAsync();
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
