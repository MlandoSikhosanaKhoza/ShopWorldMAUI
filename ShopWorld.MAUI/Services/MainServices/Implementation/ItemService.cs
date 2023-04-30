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
                    string url = (await DownloadImageBase64Async(item.ImageName)) ? Constants.GenerateImageUrl(item.ImageName) : "image_not_found.png";
                    await _itemRepository.InsertAsync(new ItemModel
                    {
                        ItemId=item.ItemId,
                        ImageName=url,
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

        public async Task<bool> ReSynchronizeItemsAsync()
        {
            await DeleteAllItemImages(); 
            bool deletedAllItems=await _itemRepository.DeleteAllAsync();
            if (deletedAllItems)
            {
                await DownloadItemsAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DownloadImageBase64Async(string ImageName)
        {
            if (!Directory.Exists(Constants.ImageDirectory))
            {
                Directory.CreateDirectory(Constants.ImageDirectory);
            }
            //Your api call for the base 64 image is added
            try
            {
                if (!System.IO.File.Exists(Constants.GenerateImageUrl(ImageName)))
                {
                    string base64 = await _shopWorldClient.Item_GetBase64ImageForImageNameAsync(ImageName);
                    byte[] image = Convert.FromBase64String(base64);
                    FileStream fileStream = new FileStream(Constants.GenerateImageUrl(ImageName), FileMode.Create);
                    fileStream.Write(image, 0, image.Length);
                    fileStream.Close();
                }
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAllItemImages()
        {
            List<ItemModel> items = await _itemRepository.GetAsync();
            try
            {
                foreach (ItemModel item in items)
                {
                    if (System.IO.File.Exists(item.ImageName))
                    {
                        System.IO.File.Delete(item.ImageName);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ItemModel>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAsync();
        }
    }
}
