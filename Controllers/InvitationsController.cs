using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.Entities;
using System.Threading.Tasks;

namespace SecretSanta.Controllers
{
    //FILTER ON WHOLE CLASS
    public class InvitationsController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IInvitationRepository _invitationRepository;
        private static User _currentUser;

        public InvitationsController(IUserRepository userRepository, IGroupRepository groupRepository, 
                                    IInvitationRepository invitationRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _invitationRepository = invitationRepository;
            //TO BE DELETED WHEN THIS COMES FROM FILTER
            _currentUser = new User();
            _currentUser.UserName = "krisi";
            _currentUser.DisplayName = "krisi";
            _currentUser.Password = "pass";
        }
        internal void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> SendInvitation([FromUri]string userName, [FromBody]string groupName)
        {
            var existingGroup = await _groupRepository.SelectByKey(groupName).ConfigureAwait(false);
            var targetUser = await _userRepository.SelectByKey(userName).ConfigureAwait(false);
            if(existingGroup == null || targetUser == null)
            {
                return NotFound();
            }
            if(_currentUser.UserName != existingGroup.AdminName)
            {
                return Unauthorized();
            }
            Invitation prepareInvitation = new Invitation();
            prepareInvitation.GroupName = groupName;
            prepareInvitation.UserName = userName;
            await _invitationRepository.Insert(prepareInvitation);

            return Created(Request.RequestUri, prepareInvitation);

        }
        [HttpGet]
        public async Task<IHttpActionResult> GetAllInvitations([FromUri]int skip = 1, [FromUri]int take = 1,
                                                               [FromUri]string order = "A")
        {
            var allInvitations = await _invitationRepository.SelectAllByUserName(_currentUser.UserName).ConfigureAwait(false);
            if (allInvitations == null)
            {
                return Ok(new List<Invitation>());
            }
            if (order == "D")
            {
                return Ok(allInvitations.Skip((skip - 1) * take).Take(take).OrderByDescending(x => x));
            }
            return Ok(allInvitations.Skip((skip - 1) * take).Take(take).OrderBy(x => x));
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteInvitation([FromBody]string groupName)
        {
            var allInvitations = await _invitationRepository.SelectAllByUserNameAndGroupName(_currentUser.UserName,
                                                                                             groupName).ConfigureAwait(false);
            if (allInvitations == null)
            {
                return NotFound();
            }
            foreach(Invitation invitation in allInvitations)
            {
                await _invitationRepository.Delete(invitation.Id).ConfigureAwait(false);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
