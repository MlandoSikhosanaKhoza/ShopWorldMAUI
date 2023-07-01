using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IUpdateCartItemTimedService
    {
        void Execute(CartModel model);
    }
}
