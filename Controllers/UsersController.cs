using SecretSanta.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Http;
using SecretSanta.Repository;

namespace SecretSanta.Controllers
{
    public class UsersController : ApiController
    {

        private readonly UserRepository _userRepository;
        //private static List<User> users = new List<User>();
       
        UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
           // users.Add(new User("user", "dipslay", "pass"));
        }

        [HttpPost]
        public IHttpActionResult CreateUser(User user)
        {
            //if (user.DisplayName == null || user.DisplayName == "" ||
            //    user.UserName == null || user.UserName == "")
            //{
                return BadRequest();
            //}
            //if (users.Any(x => x.DisplayName == user.DisplayName))
            //{
            //    return Conflict();
            //}
            //else
            //{
            //    var newUser = new User(user.UserName, user.DisplayName, user.Password);
            //    users.Add(newUser);
            //    return Ok(new UserProfile(newUser));
            //}
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.SelectAll().ConfigureAwait(false);
        }
    }
}
