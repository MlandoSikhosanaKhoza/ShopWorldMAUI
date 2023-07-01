using CommunityToolkit.Mvvm.Messaging.Messages;
using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Messages
{
    public class DeleteCartItemMessage:ValueChangedMessage<CartModel>
    {
        public DeleteCartItemMessage(CartModel cart) : base(cart) { 
        }
    }
}
