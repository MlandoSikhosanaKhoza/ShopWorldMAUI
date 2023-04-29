using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface ISettingsService
    {
        string GetFullName();
        void SetFullName(string FullName);
    }
}
