using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class WarehouseModel
    {
        public int WarehouseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
