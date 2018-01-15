using System.Web.Http;
using SecretSanta.Entities;
using SecretSanta.Repository;
using SecretSanta.CrossDomain;
using System.Threading.Tasks;


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
    }
}
