using System.Web.Http;
using SecretSanta.Entities;
using SecretSanta.Repository;
using System.Threading.Tasks;

namespace SecretSanta.Controllers
{
    public class GroupsController : ApiController
    {
       private readonly IGroupRepository _groupRepository;
       private static User _currentUser;
        public GroupsController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        internal void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
        [HttpPost]
        public async Task<IHttpActionResult> CreateGroup(Group Group)
        {
            var checkForConflit = await _groupRepository.SelectByKey(Group.GroupName);
            if (checkForConflit != null)
            {
                return Conflict();
            }
            Group.AdminName = _currentUser.UserName;
            var groups = await _groupRepository.Insert(Group).ConfigureAwait(false);
            return Created(Request.RequestUri, Group);
        }
    }
}
