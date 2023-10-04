using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Validation
{
    public class StringPriceValid<T> : IValidationRule<string>
    {
        private readonly Regex regex = new Regex(((1.2d+"").Contains(","))?"^[0-9]*\\,[0-9]{2}$":"^[0-9]*\\.[0-9]{2}$");
        public string ValidationMessage { get ; set ; }

        public bool Check(string value)
        {
            return regex.IsMatch(value);
        }
    }
}
