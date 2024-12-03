
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class ItemModel
    {
        [PrimaryKey]
        public int ItemId { get; set; }
        /*Image url stored here*/
        [MaxLength(300)]
        public string ImageName { get; set; }
        [MaxLength(40)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateSynced { get; set; }
        [Ignore]
        public ICollection<OrderItemModel> OrderItems { get; set; }
    }
}
