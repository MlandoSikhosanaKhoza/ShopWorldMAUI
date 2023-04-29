using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class ProfileViewModel:BaseViewModel
    {
        private ISettingsService _settingsService;
        private IAuthorizationService _authorizationService;
        private INavigationService _navigationService;
        public ProfileViewModel(ISettingsService settingsService,IAuthorizationService authorizationService,INavigationService navigationService) { 
            _settingsService = settingsService;
            _authorizationService = authorizationService;
            _navigationService = navigationService;
        }
        [ObservableProperty]
        private string fullName;

        [RelayCommand]
        private async void Logout()
        {
            await _authorizationService.WipePersonalDataAsync();
            await _navigationService.NavigateToAsync($"//{nameof(StartUpPage)}");
        }

        public async void OnAppearingAsync()
        {
            FullName = _settingsService.GetFullName();
        }
    }
}
