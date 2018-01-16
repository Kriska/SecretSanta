using System.Web.Http;
using SecretSanta.Entities;
using SecretSanta.Repository;
using SecretSanta.CrossDomain;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace SecretSanta.Controllers
{
    public class GroupsController : ApiController
    {
        private readonly IRepository<Group, string> _groupRepository;
        private static User _currentUser;
        public GroupsController(IRepository<Group, string> userRepository)
        {
            _groupRepository = userRepository;
        }
        internal void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateGroup(Group Group)
        {
            Group.IdAdmin = _currentUser.Id;
            try
            {
                await _groupRepository.Insert(Group).ConfigureAwait(false);
            }
            catch (ConflictException Exc)
            {
                return Conflict();
            }

            return Created(Request.RequestUri, Group);
        }
        [HttpPost]
        public async Task<IHttpActionResult> AcceptInvitation([FromUri]string groupName, [FromBody] string userName)
        {
            if(_currentUser.UserName != userName)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            
            var groups = await _groupRepository.SelectAllInvitations(_currentUser.Id).ConfigureAwait(false);
            //those that he was invited but hasn't accepted
            var found = groups.Where(x => x.IdParticipant == 0);
            if(found.Count() <= 0)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            foreach(Group group in found)
            {
                group.IdParticipant = _currentUser.Id;
                group.IdInvited = 0;
                _groupRepository.Update(group);
            }
            return Created(Request.RequestUri, "");
        }
    }
}
