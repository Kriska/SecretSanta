using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.CrossDomain;
using SecretSanta.Entities;
using Dapper;
using System.Linq;

namespace SecretSanta.Repository
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        public GroupRepository(IDbSettings settings) : base(settings)
        {
        }
        public async Task<Group> Insert(Group entity)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"INSERT INTO groups (groupName, adminName)
				                                VALUES( @GroupName, @AdminName)", entity).ConfigureAwait(false);
                return entity;
            }
        }
        public async Task<Group> Update(Group entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(string id)
        {
            using (var connection = CreateConnection())
            {
                var logins = await connection.ExecuteAsync(
                @"DELETE FROM groups WHERE id= @Id", new { Id = id }).ConfigureAwait(false);
                return logins;
            }
        }
        public Task<IEnumerable<Group>> SelectAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Group> SelectByKey(string groupName)
        {
            using (var connection = CreateConnection())
            {
                var groups = await connection.QueryAsync<Group>(@"SELECT * FROM groups WHERE groupName = @GroupName",
                    new { GroupName = groupName }).ConfigureAwait(false);
                if (groups.ToList().Count() <= 0)
                {
                    return null;
                }
                return groups.ElementAt(0);
            }
        }
    }
}