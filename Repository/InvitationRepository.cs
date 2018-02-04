using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.CrossDomain;
using SecretSanta.Entities;
using Dapper;

namespace SecretSanta.Repository
{
    public class InvitationRepository : BaseRepository, IInvitationRepository
    {
        public InvitationRepository(IDbSettings settings) : base(settings)
        {
        }
        public async Task<Invitation> Insert(Invitation entity)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(@"INSERT INTO invitations (userName, groupName)
                                                            VALUES( @UserName, @GroupName)", entity).ConfigureAwait(false);

                return entity;
            }
        }
        public Task<Invitation> Update(Invitation entity)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id)
        {
            using (var connection = CreateConnection())
            {
                var afected = await connection.ExecuteAsync(@"DELETE FROM invitations WHERE id = @Id",
                                                    new { Id = id }).ConfigureAwait(false);
                return afected;
            }

        }

        public Task<IEnumerable<Invitation>> SelectAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> SelectAllByUserName(string userName)
        {
            using (var connection = CreateConnection())
            {
                var foundInvitations = await connection.QueryAsync<string>(@"SELECT groupName FROM invitations 
                                                                            WHERE userName = @UserName",
                                                                            new { UserName = userName } ).ConfigureAwait(false);
                if(foundInvitations.Count() <=0)
                {
                    return null;
                }
                return foundInvitations;
            }

        }

        public Task<Invitation> SelectByKey(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Invitation>> SelectAllByUserNameAndGroupName(string userName, string groupName)
        {
            using (var connection = CreateConnection())
            {
                var foundInvitations = await connection.QueryAsync<Invitation>(@"SELECT * FROM invitations 
                                                                           WHERE userName = @UserName AND groupName = @GroupName",
                                       new { UserName = userName, GroupName = groupName }).ConfigureAwait(false);
                if (foundInvitations.Count() <= 0)
                {
                    return null;
                }
                return foundInvitations.ToList();
            }
        }
    }
}