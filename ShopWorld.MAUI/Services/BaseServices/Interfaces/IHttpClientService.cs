using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IHttpClientService
    {
        /// <summary>
        /// Get the ShopWorld Client with the token if it is present
        /// </summary>
        /// <returns></returns>
        HttpClient GetShopWorldClient();
    }
}
