using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IAuthorizationService
    {
        Task SetLoginToken(string Token);
        string GetToken();
        Task ProcessTokenAsync();
        bool IsValidToken();
    }
}
