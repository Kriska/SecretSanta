using SecretSanta.CrossDomain;
using SecretSanta.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SecretSanta.Repository
{
    public class UserRepository : BaseRepository, IRepository<User, string>
    {
        public UserRepository(IDbSettings settings) : base(settings)
        {
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Insert(User entity)
        {
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User>
                     (@"SELECT * FROM users WHERE users.userName = @UserName", entity).ConfigureAwait(false);
                var found = users.ToList();
                if (found.Count() <= 0)
                {
                    throw new ConflictException("The given userName is not unique");
                }
               await connection.ExecuteAsync(@"INSERT INTO 
                    users (userName, displayName, password)
                    VALUES (@UserName, @DisplayName, @Password)",
                    entity).ConfigureAwait(false);
                return entity;
            }
        }

        public async Task<IEnumerable<User>> SelectAll()
        {
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User>("SELECT * FROM users").ConfigureAwait(false);

                return users.ToList();
            }
        }

        public async Task<User> SelectByUserName(string userName)
        {
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User>
                    (@"SELECT * FROM users WHERE users.userName = @UserName", new { UserName = userName })
                    .ConfigureAwait(false);

                var result = users.ToList();
                if(result.Count > 0)
                {
                   var foundUser = result.ElementAt(0);
                    return foundUser;
                }
                throw new NotFoundException("User not found");
            }
        }

        public Task<User> SelectById(int id)
        {
            throw new NotImplementedException();
        }
        public Task<User> Update(User entity)
        {
            throw new NotImplementedException();
        }
        public Task<User> GetUserByLogin(Login login)
        {
            //won't be implemented
            throw new NotImplementedException();
        }
        public async Task<User> SelectByToken(string token)
        {
            //wont be implemented
            throw new NotImplementedException();
        }

        public Task<User> SelectByGroupName(string groupName)
        {
            //won't be implemented
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> SelectAllInvitations(int idInvited)
        {
            //won't be implemented
            throw new NotImplementedException();
        }
    }
}