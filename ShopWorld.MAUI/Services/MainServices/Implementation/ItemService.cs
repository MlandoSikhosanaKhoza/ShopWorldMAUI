using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Swagger;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class ItemService:IItemService
    {
        private ShopWorldClient _shopWorldClient;
        private IGenericRepository<ItemModel> _itemRepository;
        public ItemService(ShopWorldClient shopWorldClient,IUnitOfWork unitOfWork) {
            _shopWorldClient = shopWorldClient;
            _itemRepository=unitOfWork.GetRepository<ItemModel>();
        }

        public async Task<bool> HasItems()
        {
            int count = await _itemRepository.CountAsync();
            bool hasItems = count>0;
            return hasItems;
        }

        public async Task<bool> CheckAndDownload()
        {
            bool hasItems = await HasItems();
            bool isDownloaded=false;
            if (!hasItems)
            {
                isDownloaded=await DownloadItemsAsync();
            }
            return isDownloaded;
        }

        public async Task<bool> DownloadItemsAsync()
        {
            try
            {
                List<Item> items=(List<Item>)await _shopWorldClient.Item_GetAllItemsAsync();
                foreach (Item item in items)
                {
                    await _itemRepository.InsertAsync(new ItemModel
                    {
                        ItemId=item.ItemId,
                        ImageName=item.ImageName,
                        Description=item.Description,
                        Price=item.Price,
                        IsDeleted=item.IsDeleted,
                        DateSynced=DateTime.UtcNow
                    });
                }
                return true;
            }
            catch (ApiException e)
            {
                
            }
            return false;
        }

        public async Task<List<ItemModel>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAsync();
        }
    }
}
