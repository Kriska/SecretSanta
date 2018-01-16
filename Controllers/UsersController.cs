using SecretSanta.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.CrossDomain;
using SecretSanta.Models;
using System.Linq;
using System.Net;

namespace SecretSanta.Controllers
{

    public class UsersController : ApiController
    {

        private readonly IRepository<User, string> _userRepository;
        private readonly IRepository<Group, string> _groupRepository;
        private static User _currentUser;

        public UsersController(IRepository<User, string> userRepository, IRepository<Group, string> groupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }
        internal void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
        [HttpPost]
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

            return Created(Request.RequestUri, new UserProfile(User));
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllUsers([FromUri]int skip = 1, [FromUri]int take = 1, [FromUri] string userName = null)
        {
            var users = await _userRepository.SelectAll().ConfigureAwait(false);
            var userProfiles = new List<UserProfile>();
            foreach (var user in users)
            {
                userProfiles.Add(new UserProfile(user));
            }
            if (userName == null)
            {
                return Ok(userProfiles.Skip((skip - 1) * take).Take(take));
            }
            var result = userProfiles.Where(x => x.UserName == userName);
            if (result.Count() > 0)
            {
                return Ok(result.Skip((skip - 1) * take).Take(take));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateInvitation([FromUri] string userName, Invitation invitation)
        {
            try
            {
                var user = await _userRepository.SelectByUserName(userName).ConfigureAwait(false);
                var group = await _groupRepository.SelectByGroupName(invitation.GroupName).ConfigureAwait(false);
                var admin = await _userRepository.SelectByUserName(invitation.AdminName).ConfigureAwait(false);
                if (group.IdAdmin == admin.Id)
                {
                    group.IdInvited = user.Id;
                } else
                {
                    return StatusCode(HttpStatusCode.Forbidden);
                }
                await _groupRepository.Update(group).ConfigureAwait(false);

                return Created(Request.RequestUri, group);
            }
            catch (NotFoundException Exc)
            {
                return NotFound();
            }


        }
        [HttpGet]
        public async Task<IHttpActionResult> CheckInvitations([FromUri] string userName, [FromUri]int skip = 1, [FromUri]int take = 1,
            [FromUri]char order = 'A')
        {
            if (_currentUser.UserName != userName)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            var groups = await _groupRepository.SelectAllInvitations(_currentUser.Id).ConfigureAwait(false);
            if (order == 'A')
            {
                return Ok(groups.Skip((skip - 1) * take).Take(take).OrderBy(x => x.GroupName));
            }
            if (order == 'D')
            {
                return Ok(groups.Skip((skip - 1) * take).Take(take).OrderByDescending(x => x.GroupName));
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> RejectInvitation([FromUri] string userName, [FromUri]string groupName)
        {
            if(_currentUser.UserName != userName)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            var groups = await _groupRepository.SelectAllInvitations(_currentUser.Id).ConfigureAwait(false);
            var found = groups.Where(x => x.GroupName == groupName);
            if (found.Count() <= 0)
            {
                return NotFound();
            }
            foreach(Group group in found)
            {
               await _groupRepository.Delete(group.Id.ToString()).ConfigureAwait(false);
            }
            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
