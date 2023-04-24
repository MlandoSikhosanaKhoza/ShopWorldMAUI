using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class HttpClientService:IHttpClientService
    {
        private readonly IAuthorizationService _authorizationService;
        public HttpClientService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public HttpClient GetShopWorldClient()
        {
            string token = _authorizationService.GetToken();

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient httpClient = new HttpClient(clientHandler);

            httpClient.BaseAddress = new Uri(Constants.ShopWorldApiUrl);
            //Check for the Singleton stored token and check if it exists

            if (string.IsNullOrEmpty(token))
            {
                return httpClient;
            }

            #region Set Authorization Token
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            #endregion Set Authorization Token
            return httpClient;
        }
    }
}
