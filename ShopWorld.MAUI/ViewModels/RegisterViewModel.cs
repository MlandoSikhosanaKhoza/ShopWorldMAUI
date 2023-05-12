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
        public RegisterViewModel(IUserManagementService userManagementService,INavigationService navigationService) { 
            _userManagementService = userManagementService;
            _navigationService = navigationService;
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
            if (IsValid())
            {
                await _userManagementService.Register(new Customer { Name=FirstName.Value,Surname=LastName.Value,Mobile=Mobile.Value });
                await Shell.Current.DisplayAlert("Success!", "You have been successfully registers", "Continue to Login");

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
        }

        //Incase I fetch anything
        public void OnAppearingAsync()
        {
            
        }
    }
}
