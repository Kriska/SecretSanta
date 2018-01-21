using System.Web.Http;
using SecretSanta.Entities;
using SecretSanta.Repository;
using SecretSanta.CrossDomain;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System;

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
        //FILTER NEEDED
        [HttpPost]
        public async Task<IHttpActionResult> CreateGroup(Group Group)
        {
            var checkForConflit = await _groupRepository.SelectByKey(Group.GroupName);
            if (checkForConflit != null)
            {
                return Conflict();
            }
            var groups = await _groupRepository.Insert(Group).ConfigureAwait(false);
            return Created(Request.RequestUri, Group);
        }
        //    [HttpPost]
        //    public async Task<IHttpActionResult> AcceptInvitation([FromUri]string groupName, [FromBody] string userName)
        //    {
        //        if (_currentUser.UserName != userName)
        //        {
        //            return StatusCode(HttpStatusCode.Forbidden);
        //        }

        //        var groups = await _groupRepository.SelectAllInvitations(_currentUser.Id).ConfigureAwait(false);
        //        //those that he was invited but hasn't accepted
        //        if (groups == null)
        //        {
        //            return NotFound();
        //        }
        //        var found = groups.Where(x => x.IdParticipant == 0 && x.GroupName == groupName);
        //        if (found.Count() <= 0)
        //        {
        //            return StatusCode(HttpStatusCode.Forbidden);
        //        }
        //        foreach (Group group in found)
        //        {
        //            group.IdParticipant = _currentUser.Id;
        //            group.IdInvited = 0;
        //            await _groupRepository.Update(group).ConfigureAwait(false);
        //        }
        //        return Created(Request.RequestUri, "");
        //    }

        //    //[HttpPost]
        //    //public async Task<IHttpActionResult> Link([FromUri]string groupName)
        //    //{
        //    //    var groups = await _groupRepository.SelectAllByGroupName(groupName).ConfigureAwait(false);
        //    //    var myGroups = groups.Where(x => x.IdAdmin == _currentUser.Id);
        //    //    if(myGroups.Count() <= 0)
        //    //    {
        //    //        return StatusCode(HttpStatusCode.Forbidden);
        //    //    }
        //    //    var GroupsWithMembers = myGroups.Where(x => x.IdParticipant != 0 && x.IdReceiver == 0);
        //    //    if(GroupsWithMembers.Count() <= 0)
        //    //    {
        //    //        return NotFound();
        //    //    }
        //    //    return Created(Request.RequestUri, "");
        //    //}

        //    //private int GetRandomReciever(List<int> members)
        //    //{
        //    //    Random randomizer = new Random();
        //    //    int rand = randomizer.Next(0, members.Count());
        //    //    return rand;
        //    //}
    }
}
