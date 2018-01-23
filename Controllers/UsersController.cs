using SecretSanta.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.Models;
using System.Linq;
using System.Net;
using SecretSanta.CrossDomain;

namespace SecretSanta.Controllers
{

    public class UsersController : ApiController
    {

        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private static User _currentUser;

        public UsersController(IUserRepository userRepository, IGroupRepository groupRepository)
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
            
            var checkForConflit = await _userRepository.SelectByKey(User.UserName);
            if(checkForConflit != null)
            {
                return Conflict();
            }
            await _userRepository.Insert(User).ConfigureAwait(false);
           
            return Created(Request.RequestUri, new UserProfile(User));
        }
        // TODO: FILTER NEEDED
        [HttpGet]
        public async Task<IHttpActionResult> GetAllUsers([FromUri]int skip = 1, [FromUri]int take = 1, [FromUri] string userName = null,
                                                         [FromUri]string orderBy = "A")
        {
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

        //    [HttpPost]
        //    public async Task<IHttpActionResult> Invitations([FromUri] string userName, Invitation invitation)
        //    {
        //        var user = await _userRepository.SelectByUserName(userName).ConfigureAwait(false);
        //        var groups = await _groupRepository.SelectAllFreeGroupByGroupName(invitation.GroupName).ConfigureAwait(false);
        //        if(_currentUser.UserName != invitation.AdminName || user == null)
        //        {
        //            return StatusCode(HttpStatusCode.Forbidden);
        //        }
        //        if (groups == null) //no row in table with free idInvited
        //        {
        //            var newEntity = new Group();
        //            newEntity.IdInvited = user.Id;
        //            newEntity.GroupName = invitation.GroupName;
        //            newEntity.IdAdmin = _currentUser.Id;
        //            var addedEntity = await _groupRepository.Insert(newEntity).ConfigureAwait(false);
        //            if(addedEntity == null)
        //            {
        //                return Conflict();
        //            }
        //            return Created(Request.RequestUri, addedEntity);
        //        }
        //        foreach(Group group in groups) {
        //            if (group.IdAdmin == _currentUser.Id)
        //            {
        //                group.IdInvited = user.Id;
        //                await _groupRepository.Update(group).ConfigureAwait(false);
        //                return Created(Request.RequestUri, group);
        //            }
        //        }
        //         return StatusCode(HttpStatusCode.Forbidden);
        //    }

        //    [HttpGet]
        //    public async Task<IHttpActionResult> Invitations([FromUri] string userName, [FromUri]int skip = 1, [FromUri]int take = 1,
        //        [FromUri]char order = 'A')
        //    {
        //        if (_currentUser.UserName != userName)
        //        {
        //            return StatusCode(HttpStatusCode.Forbidden);
        //        }
        //        var groups = await _groupRepository.SelectAllInvitations(_currentUser.Id).ConfigureAwait(false);
        //        if(groups == null)
        //        {
        //            return Ok();
        //        }
        //        if (order == 'A')
        //        {
        //            return Ok(groups.Skip((skip - 1) * take).Take(take).OrderBy(x => x.GroupName));
        //        }
        //        if (order == 'D')
        //        {
        //            return Ok(groups.Skip((skip - 1) * take).Take(take).OrderByDescending(x => x.GroupName));
        //        }
        //        return BadRequest();
        //    }

        //    [HttpDelete]
        //    public async Task<IHttpActionResult> Invitations([FromUri] string userName, [FromUri]string groupName)
        //    {
        //        if(_currentUser.UserName != userName)
        //        {
        //            return StatusCode(HttpStatusCode.Forbidden);
        //        }
        //        var groups = await _groupRepository.SelectAllInvitations(_currentUser.Id).ConfigureAwait(false);
        //        if(groups == null)
        //        {
        //            return NotFound();
        //        }
        //        var found = groups.Where(x => x.GroupName == groupName);
        //        if (found.Count() <= 0)
        //        {
        //            return NotFound();
        //        }
        //        foreach(Group group in found)
        //        {
        //           await _groupRepository.Delete(group.Id.ToString()).ConfigureAwait(false);
        //        }
        //        return StatusCode(HttpStatusCode.NoContent);

        //    }

        //    [HttpGet]
        //    public async Task<IHttpActionResult> Groups([FromUri] string userName, [FromUri]int skip = 1, [FromUri]int take = 1)
        //    {
        //        if (_currentUser.UserName != userName)
        //        {
        //            return StatusCode(HttpStatusCode.Forbidden);
        //        }
        //        var groups = await _groupRepository.SelectAllByIdParticipant(_currentUser.Id).ConfigureAwait(false);
        //        if (groups == null)
        //        {
        //            return Ok();
        //        }
        //            return Ok(groups.Skip((skip - 1) * take).Take(take).OrderBy(x => x.GroupName));
        //     }
    }
}
