using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class StockItemModel
    {
        public int StockItemId { get; set; }

        public string StockItemType { get; set; }

        public int? WarehouseId { get; set; }

        public int? SupplierId { get; set; }

        public int? LogisticsId { get; set; }

        public int ItemId { get; set; }

        public int? Quantity { get; set; }

        public virtual ItemModel Item { get; set; }

        public virtual SupplierModel Supplier { get; set; }

        public virtual WarehouseModel Warehouse { get; set; }
    }
}
