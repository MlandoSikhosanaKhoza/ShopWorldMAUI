﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Services;
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
    public partial class LoginViewModel:BaseViewModel
    {
        private IUserManagementService _userManagementService;
        private INavigationService _navigationService;
        private IAuthorizationService _authorizationService;
        private IOrderService _orderService;
        private IOrderItemService _orderItemService;
        private ISettingsService _settingsService;
        public LoginViewModel(
            INavigationService navigationService, 
            IAuthorizationService authorizationService,
            IUserManagementService userManagementService,
            IOrderService orderService,IOrderItemService orderItemService,
            ISettingsService settingsService) { 
            _userManagementService= userManagementService;
            _navigationService= navigationService;
            _authorizationService= authorizationService;
            _settingsService= settingsService;
            _orderService= orderService;
            _orderItemService= orderItemService;
        }
        [ObservableProperty]
        private string mobileNumber;

        [RelayCommand]
        private async void Login()
        {
            if(IsBusy)
                return;

            IsBusy = true;
            LoginResult loginResult=await _userManagementService.LoginAsUser(MobileNumber);
            if (loginResult.IsAuthorized)
            {

                /* Download all the customer Receipts */
                await _orderService.CheckAndDownload();
                await _orderItemService.CheckAndDownload();
                
                /* Read JWT Token for Full name*/
                _settingsService.SetFullName(JwtTokenReader.GetTokenValue(loginResult.JwtToken,ClaimTypes.Name));
                
                /* Go to the Shopping Page*/
                await _navigationService.NavigateToAsync($"//{nameof(ShoppingPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Invalid Login", "Incorrect Username/Password", "OK");
            }

            IsBusy = false;
        }
    }
}