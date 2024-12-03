using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Surname { get; set; }
        [Required]
        [MaxLength(12)]
        public string? Mobile { get; set; }

        public List<OrderModel> Order { get; set; } = new List<OrderModel>();
    }
}
