
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.Entities;
using SecretSanta.CrossDomain;
using System.Threading.Tasks;
using System.Net;

namespace SecretSanta.Controllers
{
    public class LoginsController : ApiController
    {
        private readonly IRepository<Login, string> _loginRepository;
        private readonly IRepository<User, string> _userRepository;

        public LoginsController(IRepository<Login, string> loginRepository, IRepository<User, string> userRepository)
        {
            _loginRepository = loginRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
      public async Task<IHttpActionResult> Login(User user)
      {
            try
            {
                var loginUser = await _userRepository.SelectByUserName(user.UserName).ConfigureAwait(false);
                if(user.Password != loginUser.Password)
                {
                    return Unauthorized();
                }
                //imame user-a tr da mu vzemem id-to 
                Login prepareLogin = new Login(loginUser.Id);
                var login = await _loginRepository.Insert(prepareLogin).ConfigureAwait(false);
                return Created(Request.RequestUri, prepareLogin.AuthnToken);
            } catch(NotFoundException Exc)
            {
                return NotFound();
            }
        }

        //da mine prez filter po nqkakv nachin
        [HttpDelete]
        public async Task<IHttpActionResult> logout([FromUri]string userName)
        {
            //ot userName da namerish UserId i da go iztriesh toq
            try
            {
                var loginUser = await _userRepository.SelectByUserName(userName).ConfigureAwait(false);
                await _loginRepository.Delete(loginUser.Id.ToString());
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (NotFoundException Exc)
            {
                return NotFound();
            }
        }
    }
}
