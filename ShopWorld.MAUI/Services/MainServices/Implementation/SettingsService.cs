using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class SettingsService : ISettingsService
    {
        public string GetFullName()
        {
            return Preferences.Get("FullName","N/A");
        }

        public void SetFullName(string FullName)
        {
            Preferences.Set("FullName",FullName);
        }
    }
}
