using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class SupplierModel
    {
        public int SupplierId { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public List<SupplierLocationModel> SupplierLocation { get; set; } = new List<SupplierLocationModel>();
    }
}
