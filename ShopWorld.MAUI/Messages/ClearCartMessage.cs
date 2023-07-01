using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Messages
{
    public class ClearCartMessage:ValueChangedMessage<bool>
    {
        public ClearCartMessage(bool MustClear):base(MustClear) { }
    }
}
