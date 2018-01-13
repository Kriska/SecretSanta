using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.CrossDomain;

namespace SecretSanta.Repository
{
    public interface IRepository<TEntityType, in TKeyType>
    {
        Task<IEnumerable<TEntityType>> SelectAll();

        Task<TEntityType> SelectById(TKeyType id);

        Task<TEntityType> Insert(TEntityType entity);

        Task Update(TEntityType entity);

        Task Delete(TKeyType id);
    }
}