using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopWorld.MAUI.Services;
using ShopWorld.MAUI.Validation;
using ShopWorld.MAUI.Views;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.ViewModels
{
    public partial class RegisterViewModel:BaseViewModel
    {
        private INavigationService _navigationService;
        private IUserManagementService _userManagementService;
        private IConnectivity _connectivity;
        public RegisterViewModel(IUserManagementService userManagementService,
            INavigationService navigationService,IConnectivity connectivity) { 
            _userManagementService = userManagementService;
            _navigationService = navigationService;
            _connectivity = connectivity;
            SetupValidation();
        }

        [ObservableProperty]
        private ValidatableObject<string> firstName=new ValidatableObject<string>();
        [ObservableProperty]
        private ValidatableObject<string> lastName = new ValidatableObject<string>();
        [ObservableProperty]
        private ValidatableObject<string> mobile = new ValidatableObject<string>();

        private List<ValidatableObject<string>> ValidationFields=new List<ValidatableObject<string>>();

        [RelayCommand]
        private async void Login()
        {
            if (IsBusy)
            {
                return;
            }
            await _navigationService.NavigateToAsync($"//{nameof(LoginPage)}");
        }
        
        [RelayCommand]
        private async void Register()
        {
            if (IsBusy) {
                return;
            }
            IsBusy = true;
            if (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (IsValid())
                { 
                    if(!await _userManagementService.MobileNumberExists(Mobile.Value))
                    {
                        await _userManagementService.Register(new Customer { Name = FirstName.Value, Surname = LastName.Value, Mobile = Mobile.Value });
                        await Shell.Current.DisplayAlert("Success!", "You have been successfully registers", "Continue to Login");
                        await _navigationService.NavigateToAsync($"//{nameof(LoginPage)}");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Mobile number already exists!", "Please ensure you use a mobile number that hasn't been used before.", "Try again");
                    }
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Connect to the Internet", "You have a problem with your connection", "OK");
            }
            IsBusy = false;
        }

        private bool IsValid()
        {
            bool isValid = true;
            foreach (ValidatableObject<string> field in ValidationFields)
            {
                field.Validate();
                if (!field.IsValid)
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        private void SetupValidation()
        {
            ValidationFields.AddRange(new List<ValidatableObject<string>> { FirstName,LastName,Mobile });
            
            /* Make all fields required */
            foreach (var field in ValidationFields) {
                field.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Required *" });
            }
            /* Finally add field specific validation */
            Mobile.Validations.Add(new NumbersOnlyRule<string> { ValidationMessage = "Only numbers allowed" });
            Mobile.Validations.Add(new MinLengthRule<string> { MinLength=10, ValidationMessage = $"Mobile number requires at least {10} numbers" });
            Mobile.Validations.Add(new MaxLengthRule<string> { MaxLength=12, ValidationMessage = $"Mobile number can have a maximum of {12} numbers" });
        }

    }
}
