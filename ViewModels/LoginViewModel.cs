using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class LoginViewModel:BaseViewModel
    {
        private IUserManagementService _userManagementService;
        private INavigationService _navigationService;
        private IAuthorizationService _authorizationService;
        public LoginViewModel(
            INavigationService navigationService, 
            IAuthorizationService authorizationService,
            IUserManagementService userManagementService) { 
            _userManagementService= userManagementService;
            _navigationService= navigationService;
            _authorizationService= authorizationService;
        }
        [ObservableProperty]
        private string mobileNumber;

        [RelayCommand]
        private async void Login()
        {
            LoginResult loginResult=await _userManagementService.LoginAsUser(MobileNumber);
            if (loginResult.IsAuthorized)
            {
                await _authorizationService.SetLoginToken(loginResult.JwtToken);
                await _navigationService.NavigateToAsync($"//{nameof(ShoppingPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Invalid Login", "Incorrect Username/Password", "OK");
            }
        }
    }
}
