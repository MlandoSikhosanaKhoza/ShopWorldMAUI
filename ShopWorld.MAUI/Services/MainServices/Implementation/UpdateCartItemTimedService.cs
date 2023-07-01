using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class UpdateCartItemTimedService:IUpdateCartItemTimedService
    {
        private readonly ICartService _cartService;
        public UpdateCartItemTimedService(ICartService cartService) { 
            _cartService = cartService;
        }

        private Dictionary<int,CartModel> ObjectsToUpdate=new Dictionary<int,CartModel>();
        private Dictionary<int,IDispatcherTimer> TimersToExecute=new Dictionary<int,IDispatcherTimer>();
        
        public void Execute(CartModel model)
        {
            if (!ObjectsToUpdate.ContainsKey(model.ItemId))
            {
                ObjectsToUpdate.Add(model.ItemId, new CartModel
                {
                    CartId =    model.CartId,
                    ItemId =    model.ItemId,
                    ItemName =  model.ItemName,
                    Quantity =  model.Quantity,
                    Price =     model.Price,
                    OrderDate = model.OrderDate
                });
                TimersToExecute.Add(model.ItemId, Application.Current.Dispatcher.CreateTimer());
                TimersToExecute[model.ItemId].Interval = TimeSpan.FromMilliseconds(300);
                TimersToExecute[model.ItemId].IsRepeating = false;
                TimersToExecute[model.ItemId].Tick += async (s, e) => { 
                    await _cartService.UpdateCartItem(model);
                    ObjectsToUpdate.Remove(model.ItemId);
                    TimersToExecute.Remove(model.ItemId);
                };
                TimersToExecute[model.ItemId].Start();
            }
            else
            {
                TimersToExecute[model.ItemId].Stop();
                TimersToExecute[model.ItemId].Start();
            }
        }
    }
}
