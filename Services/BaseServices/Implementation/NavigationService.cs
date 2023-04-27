using ShopWorld.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class NavigationService:INavigationService
    {
        private readonly IAuthorizationService _authorizationService;

        public NavigationService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public Task InitializeAsync() =>
            NavigateToAsync(
                string.IsNullOrEmpty(_authorizationService.GetToken())
                    ? $"//{nameof(StartUpPage)}"
                    : $"//{nameof(ShoppingPage)}");

        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            var shellNavigation = new ShellNavigationState(route);

            return routeParameters != null
                ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
                : Shell.Current.GoToAsync(shellNavigation);
        }

        public Task PopAsync() =>
            Shell.Current.GoToAsync("..");
    }
}
