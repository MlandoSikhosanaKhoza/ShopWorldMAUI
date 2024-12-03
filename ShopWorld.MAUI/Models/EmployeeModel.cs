using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool IsDeleted { get; set; }

    }
}
