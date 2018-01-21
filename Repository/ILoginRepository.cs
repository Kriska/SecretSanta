using System.Threading.Tasks;
using SecretSanta.Entities;

namespace SecretSanta.Repository
{
    public interface ILoginRepository : IRepository<Login, string>
    {
        Task<Login> SelectByUserName(string userName);
    }
}