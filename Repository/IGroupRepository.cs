using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Entities;

namespace SecretSanta.Repository
{
    public interface IGroupRepository : IRepository<Group, string>
    {
    }
}