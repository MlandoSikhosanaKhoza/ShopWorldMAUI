
using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ShopWorld.MAUI.Models
{
    public class OrderModel
    {
        [PrimaryKey]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? DateFulfilled { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid OrderReference { get; set; }
        public decimal VAT { get; set; } = Tax.VAT;
        public decimal Subtotal { get; set; }
        public decimal GrandTotal { get; set; }
        [Ignore]
        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
