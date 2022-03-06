using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactShope.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {


        [HttpGet]
        [Route("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet]
        [Route("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("Bad request");
        }

        [HttpGet]
        [Route("unauth")]
        public ActionResult GetUnauthroised()
        {
            return Unauthorized();
        }


    }
}
