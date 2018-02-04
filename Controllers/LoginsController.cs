
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.Entities;
using System.Threading.Tasks;
using System;
using System.Net;

namespace SecretSanta.Controllers
{
    public class LoginsController : ApiController
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IUserRepository _userRepository;
        private User _currentUser;

        public LoginsController(ILoginRepository loginRepository, IUserRepository userRepository)
        {
            _loginRepository = loginRepository;
            _userRepository = userRepository;
        }
        internal void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Login(User user)
        {
           var loginUser = await _userRepository.SelectByKey(user.UserName).ConfigureAwait(false);
            if(loginUser == null)
            {
                return Unauthorized();
            }
            if (user.Password != loginUser.Password)
            {
                return Unauthorized();
            }
            string generatedToken = Guid.NewGuid().ToString("N");
            Login prepareLogin = new Login(user.UserName, generatedToken);
            var login = await _loginRepository.Insert(prepareLogin).ConfigureAwait(false);

            return Created(Request.RequestUri, prepareLogin.AuthnToken);
        }
        //FILTERED
        [HttpDelete] 
        public async Task<IHttpActionResult> Logout()
        {
            var result = await _loginRepository.Delete(_currentUser.UserName);
            if (result <= 0)
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
           
        }
    }
}
