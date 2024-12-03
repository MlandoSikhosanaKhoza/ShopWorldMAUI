using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class LogisticsModel
    {
        public int LogisticsId { get; set; }

        public string VehicleDescription { get; set; }

        public string VehicleNumberPlate { get; set; }

        public string FromString { get; set; }

        public int? WarehouseFromId { get; set; }

        public int? SupplierFromId { get; set; }

        public decimal? SupplierCost { get; set; }

        public int WarehouseDeliveryId { get; set; }

        public DateTime? WarehouseDispatchDate { get; set; }

        public DateTime? DateDelivered { get; set; }

        public DateTime? DueDate { get; set; }

        public SupplierModel SupplierFrom { get; set; }

        public WarehouseModel WarehouseDelivery { get; set; }

        public WarehouseModel WarehouseFrom { get; set; }
    }
}
