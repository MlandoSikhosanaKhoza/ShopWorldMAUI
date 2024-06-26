﻿using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public interface IUserManagementService
    {
        Task<bool> MobileNumberExists(string Mobile);
        Task<Customer> Register(Customer CustomerObj);
        Task<LoginResult> LoginAsUser(string Mobile);
        Task<LoginResult> LoginAsAdmin();
        Task Logout();
    }
}
