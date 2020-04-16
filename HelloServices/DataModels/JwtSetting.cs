using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloServices.DataModels
{
    public class JwtSetting
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }

        public long ExpireSeconds { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public SigningCredentials Credentials
        {
            get
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
                return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            }
        }
    }
}
