using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecretSanta.Repository;
using SecretSanta.Entities;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SecretSanta.Controllers
{
    public class LinksController : ApiController
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly ILinkRepository _linkRepository;
        private readonly IGroupRepository _groupRepository;
        private static User _currentUser;

        public LinksController(IInvitationRepository invitationRepository, ILinkRepository linkRepository,
                               IGroupRepository groupRepository)
        {
            _invitationRepository = invitationRepository;
            _linkRepository = linkRepository;
            _groupRepository = groupRepository;
        }
        internal void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        [HttpPost]
        public async Task<IHttpActionResult> AcceptInvitation([FromBody]string groupName)
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

            return Created(Request.RequestUri, prepareLink.GroupName);
        }

        [HttpPut]
        public async Task<IHttpActionResult> LinkUsers([FromBody]string groupName)
        {
            var resultsForGroup = await _linkRepository.SelectAllByGroupName(groupName).ConfigureAwait(false);
            if (resultsForGroup == null)
            {
                NotFound();
            }
            var processStarted = resultsForGroup.ToList().FindIndex(x => x.SenderName != null);
            if (processStarted >= 0 || resultsForGroup.Count() <= 1)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            var trueGroup = await _groupRepository.SelectByKey(groupName).ConfigureAwait(false);
            if (trueGroup.AdminName != _currentUser.UserName)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            List<string> allSenders = new List<string>();
            foreach (Link link in resultsForGroup)
            {
                allSenders.Add(link.RecieverName);
            }
            int index = 0;
            Random rnd = new Random();
            while (allSenders.Any())
            {
                int random = rnd.Next(0, allSenders.Count());
                var candidateSender = allSenders.ElementAt(random);
                var currentLink = resultsForGroup.ElementAt(index);
                if (currentLink.RecieverName != candidateSender)
                {
                    currentLink.SenderName = candidateSender;
                    allSenders.RemoveAt(random);
                    index++;
                }
                else if (allSenders.Count() > random + 1)
                {
                    random++;
                    currentLink.SenderName = candidateSender;
                    allSenders.RemoveAt(random);
                    index++;
                }
                else if (0 >= random - 1)
                {
                    random++;
                    currentLink.SenderName = candidateSender;
                    allSenders.RemoveAt(random);
                    index++;
                }

            }
            foreach (Link link in resultsForGroup)
            {
                await _linkRepository.Update(link);
            }
            return Created(Request.RequestUri, "");
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllUserGroups([FromUri]string groupName = null,
                                                              [FromUri]int skip = 1, [FromUri]int take = 1)
        {
            IEnumerable<Link> result;
            IEnumerable<Link> userGroups;
            bool isAdmin = false;
            bool checkForSender = false;

            if (groupName == null)
            {
                result = await _linkRepository.SelectAll().ConfigureAwait(false);
            }
            else
            {
                result = await _linkRepository.SelectAllByGroupName(groupName);
                var trueGroup = await _groupRepository.SelectByKey(groupName).ConfigureAwait(false);
                isAdmin = (trueGroup.AdminName == _currentUser.UserName);
                checkForSender = true;
            }
            if (result == null)
            {
                result = new List<Link>();
            }
            if(!isAdmin)
            {
                userGroups = result.Where(x => x.RecieverName == _currentUser.UserName);
                if (userGroups == null || result.Count() <= 0)
                {
                    userGroups = new List<Link>();
                }
                if(checkForSender)
                {
                    return Ok(userGroups.Select(o => o.SenderName).Skip((skip - 1) * take).Take(take).OrderBy(x => x));
                }
                return Ok(userGroups.Select(o => o.GroupName).Skip((skip - 1) * take).Take(take).OrderBy(x => x));
            }
            return Ok(result.Select(o => o.RecieverName).Skip((skip - 1) * take).Take(take).OrderBy(x => x));
        }

        [HttpDelete]
        public async Task<IHttpActionResult> КickUser([FromUri]string userName, [FromUri]string groupName)
        {
            var trueGroup = await _groupRepository.SelectByKey(groupName).ConfigureAwait(false);
            if (trueGroup.AdminName != _currentUser.UserName)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            var result = await _linkRepository.SelectAllByGroupName(groupName);
            var linkToDelete = result.Where(x => x.RecieverName == userName);
            if (linkToDelete.Count() <=0 || linkToDelete == null)
            {
                return NotFound();
            }
            foreach(Link link in linkToDelete)
            {
                await _linkRepository.Delete(link.Id);
            }
            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
