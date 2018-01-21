using System.Threading.Tasks;
using System.Collections.Generic;
using SecretSanta.Entities;

namespace SecretSanta.Repository
{
    public interface ILinkRepository : IRepository<Link, int>
    {
        Task<IEnumerable<Link>> SelectAllByGroupName(string groupName);
        Task<IEnumerable<Link>> SelectByRecieverAndGroup(string recieverName, string groupName);
    }
}
