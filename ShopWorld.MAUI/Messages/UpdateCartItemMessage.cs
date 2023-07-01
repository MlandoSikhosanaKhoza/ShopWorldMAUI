using CommunityToolkit.Mvvm.Messaging.Messages;
using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Messages
{
    public class UpdateCartItemMessage:ValueChangedMessage<CartModel>
    {
        public UpdateCartItemMessage(CartModel model):base(model) { 
        }
    }
}
