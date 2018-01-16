using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Entities;

namespace SecretSanta.Repository
{
    public interface IRepository<TEntityType, in TKeyType>
    {
        Task<IEnumerable<TEntityType>> SelectAll();

        Task<TEntityType> SelectByUserName(string userName); //for userRepository

        Task<TEntityType> SelectByToken(TKeyType id); //for loginRepository
        Task<TEntityType> SelectByGroupName(TKeyType groupName); //for groupRepository
        Task<IEnumerable<TEntityType>> SelectAllInvitations(int idInvited); //for groupRepository

        Task<User> GetUserByLogin(Login login);

        Task<TEntityType> Insert(TEntityType entity);

        Task<TEntityType> Update(TEntityType entity);

        Task Delete(TKeyType id);
    }
}