using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SecretSanta.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Login(User user)
        {
            //if user is in repository
            //if no matching password -> return Unauthorized();
            //return authnToken
            //else
            return NotFound();

        }

        [HttpDelete]
        public IHttpActionResult logout([FromUri]string userName)
        {
            //delete user from repository table login
            return BadRequest();
        }
    }
}
