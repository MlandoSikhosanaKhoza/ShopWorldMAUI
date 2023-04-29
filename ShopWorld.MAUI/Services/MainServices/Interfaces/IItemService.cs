﻿using ShopWorld.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IItemService
    {
        Task<bool> HasItems();
        Task<bool> CheckAndDownload();
        Task<bool> DownloadItemsAsync();
        Task<bool> ReSynchronizeItemsAsync();
        Task<List<ItemModel>> GetAllItemsAsync();
    }
}
