using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Validation
{
    public class MinLengthRule<T> : IValidationRule<string>
    {
        public int MinLength { get; set; }= 0;
        public string ValidationMessage { get ; set ; }

        public bool Check(string value)
        {
            if(string.IsNullOrEmpty(value)) return false;
            return MinLength <= value.Length;
        }
    }
}
