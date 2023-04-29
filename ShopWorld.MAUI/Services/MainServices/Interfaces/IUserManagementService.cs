using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IUserManagementService
    {
        Task<LoginResult> LoginAsUser(string Mobile);
        Task Logout();
    }
}
