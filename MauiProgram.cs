using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif
        #region Services
        /* Setup Services / Foundation Services */
        builder.Services.AddSingleton<IAuthorizationService,AuthorizationService>();
        builder.Services.AddSingleton<IHttpClientService,HttpClientService>();
        builder.Services.AddSingleton<INavigationService,NavigationService>();
        /* Operational Services */
        #endregion Services

        #region 
        /* Has its own navigation stack */
        builder.Services.AddSingleton<StartUpPage>();
        /* Has its own navigation stack */
        builder.Services.AddSingleton<LoginPage>();

        #endregion Views

        #region ViewModels
        builder.Services.AddSingleton<StartUpViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        #endregion ViewModels

        #region Clients
        builder.Services.AddSingleton<ShopWorldClient>();
        #endregion Clients
        return builder.Build();
	}
}
