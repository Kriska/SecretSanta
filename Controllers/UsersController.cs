using SecretSanta.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.CrossDomain;

namespace SecretSanta.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {

        private readonly IRepository<User, string> _userRepository;
        //private static List<User> users = new List<User>();
       
        public UsersController(IRepository<User,string> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateUser(User User)
        {
            try
            {
                await _userRepository.Insert(User).ConfigureAwait(false);
            }
            catch (ConflictException Exc)
            {
                return Conflict();
            }

            return Created(Request.RequestUri, User.DisplayName);
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.SelectAll().ConfigureAwait(false);
        }
    }
}
