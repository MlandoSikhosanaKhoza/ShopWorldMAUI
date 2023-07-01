using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Swagger;
using ShopWorld.MAUI.ViewModels;
using ShopWorld.MAUI.Views;

namespace ShopWorld.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-regular-900.ttf", "FontAwesome");
            });
        #region Features
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        #endregion Features

        #region Services
        /* Setup Services / Foundation Services */
        builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
        builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        /* Operational Services */
        builder.Services.AddSingleton<IUserManagementService, UserManagementService>();
        builder.Services.AddSingleton<IItemService, ItemService>();
        builder.Services.AddSingleton<ICartService, CartService>();
        builder.Services.AddSingleton<IOrderService, OrderService>();
        builder.Services.AddSingleton<IOrderItemService, OrderItemService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IUpdateCartItemTimedService,UpdateCartItemTimedService>();
        builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
        #endregion Services

        #region Views
        /* Has its own navigation stack */
        builder.Services.AddSingleton<StartUpPage>();
        /* Has its own navigation stack */
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        /*Has its own navigation stacks*/
        builder.Services.AddSingleton<ShoppingPage>();
        builder.Services.AddTransient<ShoppingCartPage>();
        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddSingleton<ReceiptPage>();
        builder.Services.AddSingleton<ReceiptDetailPage>();
        #endregion Views

        #region ViewModels
        builder.Services.AddSingleton<StartUpViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<RegisterViewModel>();

        builder.Services.AddSingleton<ShoppingViewModel>();
        /* Shopping Cart is dependant on the ShoppingViewModel its based on the items purchased inside the ShoppingViewModel */
        builder.Services.AddSingleton<ShoppingCartViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddSingleton<ReceiptViewModel>();
        builder.Services.AddTransient<ReceiptDetailViewModel>();
        #endregion ViewModels

        #region Clients
        builder.Services.AddSingleton<ShopWorldClient>();
        #endregion Clients

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
