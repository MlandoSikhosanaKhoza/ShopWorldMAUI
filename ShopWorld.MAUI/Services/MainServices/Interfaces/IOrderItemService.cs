using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IOrderItemService
    {
        Task<bool> HasOrderItems();
        Task<bool> CheckAndDownload();
        Task<bool> DownloadOrderItemsAsync(int OrderId);
        Task<List<OrderItemModel>> GetOrderItemsByOrderAsync(int OrderId);
    }
}
