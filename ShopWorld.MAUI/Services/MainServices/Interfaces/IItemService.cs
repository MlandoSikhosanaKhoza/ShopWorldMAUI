using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.ViewModels;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IItemService
    {
        Task<bool> HasItems();
        Task<Item> AddItemAsync(ItemInputModel item);
        Task<bool> UpdateItemAsync(ItemInputModel item);
        Task<bool> CheckAndDownload();
        Task<bool> DownloadItemsAsync();
        Task<bool> DeleteAllItemImages();
        Task<bool> ReSynchronizeItemsAsync();
        Task<List<ItemModel>> GetAllItemsAsync();
        Task<ObservableCollection<BindItemViewModel>> GetAllBindableItems();
    }
}
