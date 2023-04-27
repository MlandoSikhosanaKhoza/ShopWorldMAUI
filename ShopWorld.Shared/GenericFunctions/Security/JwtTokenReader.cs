using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.Shared
{
    public class JwtTokenReader
    {
        //Read a JWT Token
        public static string GetTokenValue(string JwtToken, string TokenField)
        {
            JwtSecurityToken token = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadJwtToken(JwtToken);
            string tokenValue = token.Claims.Where(c => c.Type.Equals(TokenField)).FirstOrDefault().Value;
            return tokenValue;
        }

        public static JwtSecurityToken GetJwtToken(string JwtToken)
        {
            JwtSecurityToken token = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadJwtToken(JwtToken);
            return token;
        }
    }
}
