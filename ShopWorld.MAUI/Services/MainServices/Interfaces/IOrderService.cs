using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IOrderService
    {
        Task<bool> HasOrders();
        Task<bool> CheckAndDownload();
        Task<bool> DownloadOrdersAsync();
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<OrderModel> GetOrderModelByIdAsync(int OrderId);
    }
}
