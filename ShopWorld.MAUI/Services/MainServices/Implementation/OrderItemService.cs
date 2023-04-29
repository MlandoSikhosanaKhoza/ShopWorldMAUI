using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Swagger;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    /// <summary>
    /// Your only add to the order item repository once the purchase has been made
    /// </summary>
    public class OrderItemService:IOrderItemService
    {
        private readonly IGenericRepository<OrderModel> _orderRepository;
        private readonly IGenericRepository<OrderItemModel> _orderItemRepository;
        private ShopWorldClient _shopWorldClient;
        public OrderItemService(IUnitOfWork unitOfWork,ShopWorldClient shopWorldClient) { 
            _orderRepository = unitOfWork.GetRepository<OrderModel>();
            _orderItemRepository = unitOfWork.GetRepository<OrderItemModel>();
            _shopWorldClient = shopWorldClient;
        }

        public async Task<bool> HasOrderItems()
        {
            int count = await _orderItemRepository.CountAsync();
            bool hasOrderItems = count > 0;
            return hasOrderItems;
        }

        public async Task<bool> CheckAndDownload()
        {
            bool hasItems = await HasOrderItems();
            bool isDownloaded = false;
            List<OrderModel> orders=await _orderRepository.GetAsync();
            if (!hasItems)
            {
                foreach (var order in orders)
                {
                    isDownloaded = await DownloadOrderItemsAsync(order.OrderId);
                }
                
            }
            return isDownloaded;
        }

        public async Task<bool> DownloadOrderItemsAsync(int OrderId)
        {
            try
            {
                List<OrderItemResult> orderItems = (List<OrderItemResult>)await _shopWorldClient.OrderItem_GetOrderViewItemsAsync(OrderId);
                foreach (OrderItemResult orderItem in orderItems)
                {
                    await _orderItemRepository.InsertAsync(new OrderItemModel
                    {
                        OrderItemId= orderItem.OrderItemId,
                        OrderId=OrderId,
                        Description=orderItem.Description,
                        ItemId = orderItem.ItemId,
                        Quantity = orderItem.Quantity,
                        Price= orderItem.Price
                    });
                }
                return true;
            }
            catch (ApiException e)
            {

            }
            return false;
        }

        public async Task<List<OrderItemModel>> GetOrderItemsByOrderAsync(int OrderId)
        {
            return await _orderItemRepository.GetAsync(oi=>oi.OrderId==OrderId);
        }
    }
}
