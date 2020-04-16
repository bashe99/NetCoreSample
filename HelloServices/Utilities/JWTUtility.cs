using HelloServices.DataModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HelloServices.Utilities
{
    public class JWTUtility: IJWTUtility
    {
        private readonly JwtSetting jwtSetting;

        public JWTUtility(IOptions<JwtSetting> option)
        {
            jwtSetting = option.Value;
        }

        public string GetToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim("roles", "Admin")
            };

            var userClaimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSetting.Issuer,
                Audience = jwtSetting.Audience,
                Subject = userClaimsIdentity,
                Expires = DateTime.Now.AddSeconds(jwtSetting.ExpireSeconds),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }

        public async Task<bool> LoginAsync(User user)
        {
            await Task.CompletedTask;
            if (user.Name == "bashe" && user.Password == "123")
            {
                return true;
            }

            return false;
        }
    }
}
