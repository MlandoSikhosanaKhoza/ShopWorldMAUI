using ShopWorld.Shared.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class OrderItemModel
    {
        [PrimaryKey]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }

        [Ignore]
        public decimal UnitPrice => Price * Quantity;
    }
}
