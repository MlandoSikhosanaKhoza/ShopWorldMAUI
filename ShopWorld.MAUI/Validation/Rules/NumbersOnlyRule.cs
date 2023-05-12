using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Validation
{
    public class NumbersOnlyRule<T> : IValidationRule<string>
    {
        private readonly Regex regex = new Regex("^[0-9]*$");
        public string ValidationMessage { get ; set ; }

        public bool Check(string value)
        {
            if(value == null) return false;
            return regex.IsMatch(value);
        }
    }
}
