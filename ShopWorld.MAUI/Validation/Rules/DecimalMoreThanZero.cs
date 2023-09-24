using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Validation
{
    public class DecimalMoreThanZero<T> : IValidationRule<decimal>
    {
        public string ValidationMessage { get; set; }
        public bool Check(decimal value)
        {
            return value > 0;
        }
    }
}
