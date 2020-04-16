using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloServices.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HelloServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {

        private IMessage message { get; set; }

        private Func<string, IMessage> messageAccesor { get; set; }

        public HelloController(IMessage message, Func<string, IMessage> messageAccesor)
        {
            this.message = message;
            this.messageAccesor = messageAccesor;
        }

        // GET api/hello
        [HttpGet]
        // [HttpGet("custom")] custom route, if set this, the request uri will become api/hello/custom
        // [Route("custom")] custom route, if set this, the request uri will become api/hello/custom
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { this.message.getValue() };
        }

        [HttpGet("another")]
        public ActionResult<IEnumerable<string>> GetAnother()
        {
            return new string[] { this.messageAccesor("anotherMessage").getValue() };
        }

        [HttpGet("checkJWT")]
        [Authorize]
        public ActionResult<string> CheckJWT()
        {
            return "get it";
        }

        [HttpGet("default")]
        public ActionResult<IEnumerable<string>> GetDefault()
        {
            return new string[] { this.messageAccesor("message").getValue() };
        }

        [HttpPost("DataAnnotationAction")]
        public ActionResult<string> DataAnnotationAction(DataAnnotationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return  $"name: {model.Name}, email: {model.Email}";
        }

        // PUT api/hello/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/hello/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
