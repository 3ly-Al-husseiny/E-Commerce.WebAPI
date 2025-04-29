using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfoud")] // GET : /api/Buggy/not found
        public IActionResult GetNotFoundRequest() 
        {
            // Code
            return NotFound(); //404
        }

        [HttpGet("servererror")] // GET : /api/Buggy/not found
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception();
            return Ok();
        }

        [HttpGet(template:"badrequest")] // GET : /api/Buggy/not found
        public IActionResult GetBadRequest()
        {
            // Code
            return BadRequest(); //404
        }

        [HttpGet(template: "badrequest/{id}")] // GET : /api/Buggy/not found
        public IActionResult GetBadRequest(int id)
        {
            // Code
            return BadRequest(); //404
        }

        [HttpGet(template: "unauthorized")] // GET : /api/Buggy/not found
        public IActionResult GetUnauthorizedRequest()
        {
            // Code
            return BadRequest(); //401
        }



    }
}
