﻿using ShopWorld.Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Services
{
    public class AuthorizationService:IAuthorizationService
    {
        private string Token { get; set; }
        public async Task SetLoginToken(string Token)
        {
            this.Token = Token;
            await SecureStorage.SetAsync("login_token", Token);
        }

        public string GetToken()
        {
            return this.Token;
        }

        public async Task ProcessTokenAsync()
        {
            Token = await SecureStorage.GetAsync("login_token");
        }

        public bool IsValidToken()
        {
            if (!string.IsNullOrEmpty(this.Token))
            {
                JwtSecurityToken jwtSecurityToken =JwtTokenReader.GetJwtToken(this.Token);
                if (DateTime.UtcNow < jwtSecurityToken.ValidTo)
                {
                    return true;
                }
            }
            return false;
        }
    }
}