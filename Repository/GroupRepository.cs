using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.CrossDomain;
using SecretSanta.Entities;
using Dapper;
using System.Linq;

namespace SecretSanta.Repository
{
    public class GroupRepository : BaseRepository, IRepository<Group, string>

    {
        public GroupRepository(IDbSettings settings) : base(settings)
        {
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Group> Insert(Group entity)
        {
            
            using (var connection = CreateConnection())
            {
                var groups = await connection.QueryAsync<Group>
                    (@"SELECT * FROM groups WHERE groups.groupName = @GroupName", entity).ConfigureAwait(false);
                var found = groups.ToList();
                if (found.Count() > 0)
                {
                    throw new ConflictException("The given groupName is not unique");
                }
                await connection.ExecuteAsync(
                @"INSERT INTO groups (idAdmin, groupName)
				VALUES( @IdAdmin, @GroupName)", entity).ConfigureAwait(false);

                return entity;
            }
        }

        public Task<IEnumerable<Group>> SelectAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Group> SelectByGroupName(string groupName)
        {
            using (var connection = CreateConnection())
            {
                var groups = await connection.QueryAsync<Group>
                    (@"SELECT * FROM groups WHERE groupName = @GroupName AND idParticipant is NULL",
                     new { GroupName = groupName}).ConfigureAwait(false);
                var found = groups.ToList();
                if (found.Count() <= 0)
                {
                    var group = await connection.QueryAsync<Group>
                    (@"SELECT * FROM groups WHERE groupName = @GroupName",
                     new { GroupName = groupName }).ConfigureAwait(false);
                    if(group.Count() > 0)
                    {
                        await connection.ExecuteAsync(
                        @"INSERT INTO groups (idAdmin, groupName) VALUES( @IdAdmin, @GroupName)", 
                        new { group.ElementAt(0).IdAdmin, GroupName = groupName }).ConfigureAwait(false);
                        return group.ElementAt(0);
                    } else
                    {
                        throw new NotFoundException("No such group");
                    }

                }
                return found.ElementAt(0);
            }
        }

        public Task<Group> SelectByToken(string id)
        {
            //won't be implemented
            throw new NotImplementedException();
        }

        public Task<Group> SelectByUserName(string userName)
        {
            //won't be implemented
            throw new NotImplementedException();
        }
        public Task<User> GetUserByLogin(Login login)
        {
            //won't be implemented
            throw new NotImplementedException();
        }
        public async Task<Group> Update(Group entity)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"UPDATE groups set idParticipant = @IdParticipant
                    WHERE groups.groupName = @GroupName and idParticipant is NULL", entity).ConfigureAwait(false);

                return entity;
            }
        }
    }
}