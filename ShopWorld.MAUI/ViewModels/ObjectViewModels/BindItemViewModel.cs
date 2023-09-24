using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Models;
using ShopWorld.MAUI.Validation;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class BindItemViewModel:ObservableObject
    {
        public BindItemViewModel(ItemModel item) { 
            _item= item;
            price = item.Price.ToString("0.00");
            O
            DescriptionCheck.Validations.Add(new IsNotNullOrEmptyRule<string>() { ValidationMessage="Required * "});
            PriceCheck.Validations.Add(new DecimalMoreThanZero<decimal>() { ValidationMessage="Price has to be more than zero"});
        }
        private ItemModel _item;

        public int ItemId {
            get { return _item.ItemId; }
            set { _item.ItemId = value; OnPropertyChanged(nameof(ItemId)); }
        }
        
        public string ImageName {
            get { return _item.ImageName; }
            set { _item.ImageName = value; OnPropertyChanged(nameof(ImageName)); }
        }
        
        public string Description { 
            get { return _item.Description; } 
            set { _item.Description = value;DescriptionCheck.Value = value; OnPropertyChanged(nameof(Description)); } 
        }

        public ValidatableObject<string> DescriptionCheck = new ValidatableObject<string>();
        private string price;
        public string Price
        {
            get { return price; }
            set {
                decimal outPrice=0;
                decimal.TryParse(value, out outPrice);
                if(outPrice > 0 && !value.EndsWith(",") && !value.EndsWith("."))
                {
                    _item.Price = outPrice;
                    PriceCheck.Value = outPrice;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }
        public ValidatableObject<decimal> PriceCheck=new ValidatableObject<decimal>();

        [RelayCommand]
        private void Save()
        {

        }
    }
}
