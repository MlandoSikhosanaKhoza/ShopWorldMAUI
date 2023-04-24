using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Views.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public StartUpViewModel(IAuthorizationService authorizationService) { 
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private async void LoginAsUser()
        {
            SimplePopup popup = new SimplePopup();
            await Shell.Current.ShowPopupAsync(popup);
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
            await _authorizationService.ProcessTokenAsync();
            if (_authorizationService.IsValidToken())
            {

            }
            IsBusy= false;
        }
    }
}
