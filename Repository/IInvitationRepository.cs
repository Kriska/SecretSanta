using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Entities;

namespace SecretSanta.Repository
{
    public interface IInvitationRepository : IRepository<Invitation, int>
    {
        Task<IEnumerable<string>> SelectAllByUserName(string userName);
        Task<IEnumerable<Invitation>> SelectAllByUserNameAndGroupName(string userName, string groupName);

    }
}
