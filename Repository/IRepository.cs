using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Entities;

namespace SecretSanta.Repository
{
    public interface IRepository<TEntityType, in TKeyType>
    {
        Task<TEntityType> Insert(TEntityType entity);
        Task<TEntityType> SelectByKey(TKeyType key);
        Task<TEntityType> Update(TEntityType entity);
        Task Delete(TKeyType id);
        Task<IEnumerable<TEntityType>> SelectAll();
    }
}