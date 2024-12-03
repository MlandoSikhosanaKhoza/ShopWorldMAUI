using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Swagger;
using ShopWorld.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class OrderService:IOrderService
    {
        private IGenericRepository<OrderModel> _orderRepository;
        private IAuthorizationService _authorizationService;
        private ShopWorldClient _shopWorldClient;
        public OrderService(IUnitOfWork unitOfWork,IAuthorizationService authorizationService,ShopWorldClient shopWorldClient)
        {
            _orderRepository = unitOfWork.GetRepository<OrderModel>();
            _shopWorldClient = shopWorldClient;
            _authorizationService  = authorizationService;
        }

        public async Task<OrderModel> GetOrderModelByIdAsync(int OrderId)
        {
            return (await _orderRepository.GetAsync(o=>o.OrderId == OrderId)).FirstOrDefault();
        }

        public async Task<bool> HasOrders()
        {
            int count = await _orderRepository.CountAsync();
            bool hasOrders = count > 0;
            return hasOrders;
        }

        public async Task<bool> CheckAndDownload()
        {
            bool hasOrders = await HasOrders();
            bool isDownloaded = false;
            if (!hasOrders)
            {
                isDownloaded = await DownloadOrdersAsync();
            }
            return isDownloaded;
        }

        public async Task<bool> DownloadOrdersAsync()
        {
            try
            {
                string strCustomerId = JwtTokenReader.GetTokenValue(_authorizationService.GetToken(), "CustomerId");
                int customerId = int.Parse(strCustomerId);
                List<OrderModel> ongoing = (List<OrderModel>)await _shopWorldClient.Order_GetOngoingOrdersForCustomerAsync(customerId);
                List<OrderModel> complete = (List<OrderModel>)await _shopWorldClient.Order_GetCompleteOrdersForCustomerAsync(customerId);
                List<OrderModel> receipts = new List<OrderModel>();
                receipts.AddRange(ongoing);
                receipts.AddRange(complete);
                foreach (OrderModel item in receipts)
                {
                    await _orderRepository.InsertAsync(new OrderModel { 
                        OrderId        = item.OrderId,
                        CustomerId     = item.CustomerId,
                        EmployeeId     = item.EmployeeId,
                        DateFulfilled  = item.DateFulfilled,
                        DateCreated    = item.DateCreated,
                        OrderReference = item.OrderReference,
                        VAT            = item.VAT,
                        Subtotal       = item.Subtotal,
                        GrandTotal     = item.GrandTotal
                    });
                }
                return true;
            }
            catch (ApiException e)
            {

            }
            return false;
        }

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAsync();
        }
    }
}
