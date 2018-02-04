using SecretSanta.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.Models;
using System.Linq;

namespace SecretSanta.Controllers
{

    public class UsersController : ApiController
    {

        private static IUserRepository _userRepository;
        private static User _currentUser;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        internal void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(User User)
        {
            
            var checkForConflit = await _userRepository.SelectByKey(User.UserName);
            if(checkForConflit != null)
            {
                return Conflict();
            }
            await _userRepository.Insert(User).ConfigureAwait(false);
           
            return Created(Request.RequestUri, new UserProfile(User));
        }
        //FILTERED
        [HttpGet]
        public async Task<IHttpActionResult> GetAllUsers([FromUri]int skip = 1, [FromUri]int take = 1, [FromUri] string userName = null,
                                                         [FromUri]string orderBy = "A")
        {
            if (_currentUser == null)
            {
                return Unauthorized();
            }
            var users = await _userRepository.SelectAll().ConfigureAwait(false);
            var userProfiles = new List<UserProfile>();
            foreach (var user in users)
            {
                userProfiles.Add(new UserProfile(user));
            }
            if (userName == null)
            {
                if(orderBy == "D")
                {
                    return Ok(userProfiles.Skip((skip - 1) * take).Take(take).OrderByDescending(x => x.DisplayName));
                }
                else
                {
                    return Ok(userProfiles.Skip((skip - 1) * take).Take(take).OrderBy(x => x.DisplayName));
                }
            }
            var result = userProfiles.Where(x => x.UserName == userName);
            if (result.Count() > 0)
            {
                return Ok(result.Skip((skip - 1) * take).Take(take));
            }
            return NotFound();
        }
    }
}
