using System;
using System.Linq;
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
        public async Task<Link> Update(Link entity)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"UPDATE links SET senderName = @SenderName
                                                WHERE id = @Id", entity).ConfigureAwait(false);

                return entity;
            }
        }
        public async Task<int> Delete(int id)
        {
            using (var connection = CreateConnection())
            {
               var affected =  await connection.ExecuteAsync(@"DELETE FROM links WHERE id= @Id",
                                                new { Id = id }).ConfigureAwait(false);
                return affected;
            }
        }

        public async Task<IEnumerable<Link>> SelectAll()
        {
            using (var connection = CreateConnection())
            {
                var results = await connection.QueryAsync<Link>(@"SELECT * FROM links").ConfigureAwait(false);
                if (results.Count() >= 0)
                {
                    return results;
                }
                return null;
            }
        }

        public async Task<IEnumerable<Link>> SelectAllByGroupName(string groupName)
        {
            using (var connection = CreateConnection())
            {
                var results = await connection.QueryAsync<Link>(@"SELECT * FROM links WHERE groupName = @GroupName", 
                                               new { GroupName = groupName }).ConfigureAwait(false);
               if(results.Count() >= 0)
                {
                    return results;
                }
                return null;
            }

        }
        public Task<Link> SelectByKey(int key)
        {
            throw new NotImplementedException();
        }

    }
}