using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopWorld.MAUI.Messages;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class SettingsViewModel:BaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IAuthorizationService _authorizationService;
        private readonly INavigationService _navigationService;
        private readonly IItemService _itemService;
        public SettingsViewModel(ISettingsService settingsService,
            IAuthorizationService authorizationService
            , INavigationService navigationService, IItemService itemService)
        {
            _settingsService = settingsService;
            _authorizationService = authorizationService;
            _navigationService = navigationService;
            _itemService = itemService;
        }

        [RelayCommand]
        private async void Logout()
        {
            await _itemService.DeleteAllItemImages();
            await _authorizationService.WipePersonalDataAsync();
            StrongReferenceMessenger.Default.Send(new LogoutMessage(true));
            await _navigationService.NavigateToAsync($"//{nameof(StartUpPage)}");
        }
    }
}
