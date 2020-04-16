using HelloServices.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloServices.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class JWTTestController : ControllerBase
    {
        private IJWTUtility jwtUtility;

        public JWTTestController(IJWTUtility jwtUtility)
        {
            this.jwtUtility = jwtUtility;
        }

        [HttpPost("getToken")]
        [AllowAnonymous]
        public async Task<string> GetTokenAsync([FromBody] User user)
        {
            var isValidUser = await this.jwtUtility.LoginAsync(user);
            if (!isValidUser)
            {
                return "user not valid";
            }

            var token = this.jwtUtility.GetToken(user);
            var response = new
            {
                Status = true,
                Token = token,
                Type = "Bearer"
            };

            return JsonConvert.SerializeObject(response);
        }
    }
}
