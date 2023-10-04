using CommunityToolkit.Mvvm.Messaging.Messages;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Messages
{
    public class SaveItemMessage : ValueChangedMessage<ItemInputModel>
    {
        public SaveItemMessage(ItemInputModel value) : base(value)
        {
        }
    }
}
