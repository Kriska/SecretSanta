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
    public class LinksController : ApiController
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly ILinkRepository    _linkRepository;
        private static User _currentUser;

        public LinksController(IInvitationRepository invitationRepository, ILinkRepository linkRepository)
        {
            _invitationRepository = invitationRepository;
            _linkRepository = linkRepository;
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
        public async Task<IHttpActionResult> acceptInvitation([FromBody]string groupName)
        {
            var allInvitations = await _invitationRepository.SelectAllByUserNameAndGroupName(_currentUser.UserName,
                                                                                            groupName).ConfigureAwait(false);
            if (allInvitations == null)
            {
                return NotFound();
            }
            foreach (Invitation invitation in allInvitations)
            {
                await _invitationRepository.Delete(invitation.Id);
            }
            Link prepareLink = new Link();
            prepareLink.RecieverName = _currentUser.UserName;
            prepareLink.GroupName = groupName;
            await _linkRepository.Insert(prepareLink);

            return Created(Request.RequestUri, prepareLink);
        }
    }
}
