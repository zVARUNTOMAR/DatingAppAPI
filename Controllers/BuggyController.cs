using DatingAppAPI.Data;
using DatingAppAPI.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseAPIController
    {
        private readonly DataContext _datacontext;
        public BuggyController(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() {
            return "Some String";
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _datacontext.Users.Find(-1);

            if (thing == null)
            {
                return NotFound();
            }
            else {
                return Ok(thing);
            }
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _datacontext.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This is not bad request");
        }

    }
}
