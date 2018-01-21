using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.CrossDomain;
using SecretSanta.Entities;
using Dapper;

namespace SecretSanta.Repository
{
    public class LinkRepository : BaseRepository, ILinkRepository
    {
        public LinkRepository(IDbSettings settings) : base(settings)
        {
        }
        public async Task<Link> Insert(Link entity)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"INSERT INTO links (recieverName, groupName)
                                                VALUES( @RecieverName, @GroupName)", entity).ConfigureAwait(false);

                return entity;
            }
        }
        public Task<Link> Update(Link entity)
        {
            throw new NotImplementedException();
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Link>> SelectAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Link>> SelectAllByGroupName(string groupName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Link>> SelectByRecieverAndGroup(string recieverName, string groupName)
        {
            throw new NotImplementedException();
        }
        public Task<Link> SelectByKey(int key)
        {
            throw new NotImplementedException();
        }

    }
}