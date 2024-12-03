using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class SupplierLocationModel
    {
        public int SupplierLocationId { get; set; }

        public int SupplierId { get; set; }

        public string Address { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
