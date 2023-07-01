using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Messages
{
    public class LogoutMessage:ValueChangedMessage<bool>
    {
        public LogoutMessage(bool IsLoggedOut):base(IsLoggedOut) { }
    }
}
