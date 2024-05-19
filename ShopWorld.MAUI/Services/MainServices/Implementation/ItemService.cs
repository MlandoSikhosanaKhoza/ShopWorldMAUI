using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Repository;
using ShopWorld.MAUI.Swagger;
using ShopWorld.MAUI.ViewModels;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class ItemService : IItemService
    {
        private ShopWorldClient _shopWorldClient;
        private IGenericRepository<ItemModel> _itemRepository;
        public ItemService(ShopWorldClient shopWorldClient, IUnitOfWork unitOfWork)
        {
            _shopWorldClient = shopWorldClient;
            _itemRepository = unitOfWork.GetRepository<ItemModel>();
        }

        public async Task<bool> HasItems()
        {
            int count = await _itemRepository.CountAsync();
            bool hasItems = count > 0;
            return hasItems;
        }

        public async Task<bool> CheckAndDownload()
        {
            bool hasItems = await HasItems();
            bool isDownloaded = false;
            if (!hasItems)
            {
                isDownloaded = await DownloadItemsAsync();
            }
            return isDownloaded;
        }

        public async Task<Item> AddItemAsync(ItemInputModel item)
        {
            try
            {
                Item itemAdded = await _shopWorldClient.Item_AddItemAsync(item);
                if (itemAdded != null)
                {
                    string base64 = item.Base64;
                    byte[] image = Convert.FromBase64String(base64);
                    FileStream fileStream = new FileStream(Constants.GenerateImageUrl(itemAdded.ImageName), FileMode.Create);
                    fileStream.Write(image, 0, image.Length);
                    fileStream.Close();
                    await _itemRepository.InsertAsync(new ItemModel
                    {
                        ItemId = itemAdded.ItemId,
                        ImageName = Constants.GenerateImageUrl(itemAdded.ImageName),
                        Description = itemAdded.Description,
                        Price = itemAdded.Price,
                        DateSynced = DateTime.Now,
                        IsDeleted = itemAdded.IsDeleted
                    });
                }
                return itemAdded;
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Request failed", $"{ex.Message}", "OK");
                return null;
            }
        }

        public async Task<bool> UpdateItemAsync(ItemInputModel item)
        {
            try
            {
                bool isUpdated = await _shopWorldClient.Item_UpdateItemAsync(item);
                if (isUpdated)
                {
                    ItemModel itemToUpdate = (await _itemRepository.GetAsync(it => it.ItemId == item.ItemId)).First();
                    if (!string.IsNullOrEmpty(item.Base64))
                    {
                        byte[] image = Convert.FromBase64String(item.Base64);
                        FileStream fileStream = new FileStream(item.ImageName, FileMode.Create);
                        fileStream.Write(image, 0, image.Length);
                        fileStream.Close();
                    }
                    itemToUpdate.Description = item.Description;
                    itemToUpdate.Price = item.Price;
                }
                return isUpdated;
            }
            catch (ApiException ex)
            {
                return false;
            }
        }

        public async Task<KeyValuePair<string,bool>> DownloadImageForItemAsync(ItemModel bindItemViewModel)
        {
            try
            {
                Item item=await _shopWorldClient.Item_GetItemAsync(bindItemViewModel.ItemId);
                string base64 = await _shopWorldClient.Item_GetBase64ImageForImageNameAsync(item.ImageName);
                byte[] image = Convert.FromBase64String(base64);
                FileStream fileStream = new FileStream(Constants.GenerateImageUrl(item.ImageName), FileMode.Create);
                fileStream.Write(image, 0, image.Length);
                fileStream.Close();
                ItemModel model = (await _itemRepository.GetAsync(i=>i.ItemId==bindItemViewModel.ItemId)).FirstOrDefault();
                model.ImageName = Constants.GenerateImageUrl(item.ImageName);
                await _itemRepository.UpdateAsync(model);
                return new KeyValuePair<string, bool>(model.ImageName,true);
            }
            catch (ApiException ex)
            {
                return new KeyValuePair<string, bool>("", false);
            }
        }

        public async Task<bool> DownloadItemsAsync()
        {
            try
            {
                List<Item> items = (List<Item>)await _shopWorldClient.Item_GetAllItemsAsync();
                foreach (Item item in items)
                {
                    string url = "image_not_found.png";
                    await _itemRepository.InsertAsync(new ItemModel
                    {
                        ItemId = item.ItemId,
                        ImageName = url,
                        Description = item.Description,
                        Price = item.Price,
                        IsDeleted = item.IsDeleted,
                        DateSynced = DateTime.UtcNow
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
            bool deletedAllItems = await _itemRepository.DeleteAllAsync();
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

        public async Task<ObservableCollection<BindItemViewModel>> GetAllBindableItems()
        {
            ObservableCollection<BindItemViewModel> items = new ObservableCollection<BindItemViewModel>();
            try
            {
                List<ItemModel> itemModels = await _itemRepository.GetAsync();
                foreach (ItemModel item in itemModels)
                {
                    items.Add(new BindItemViewModel(item));
                }
            }
            catch (Exception e)
            {

            }

            return items;
        }

        public async Task<List<ItemModel>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAsync();
        }
    }
}
