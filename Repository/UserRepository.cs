using SecretSanta.CrossDomain;
using SecretSanta.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SecretSanta.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbSettings settings) : base(settings)
        {
        }
        public async Task<User> Insert(User entity)
        {
            using (var connection = CreateConnection())
            {
             await connection.ExecuteAsync(@"INSERT INTO users (userName, displayName, password)
                                            VALUES (@UserName, @DisplayName, @Password)", entity).ConfigureAwait(false);
                return entity;
            }
        }
        public async Task<User> SelectByKey(string key)
        {
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User>
                     (@"SELECT * FROM users WHERE users.userName = @UserName", new { Username = key }).ConfigureAwait(false);
                if(users.Count() <= 0)
                {
                    return null;
                }
                var result = users.ElementAt(0);
                return result;
            }
        }

        public Task<User> Update(User entity)
        {
            throw new NotImplementedException();
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> SelectAll()
        {
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User>("SELECT * FROM users").ConfigureAwait(false);

                return users.ToList();
            }
        }
    }
}