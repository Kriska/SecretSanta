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
                var users = await connection.QueryAsync<User>("SELECT * FROM users").ConfigureAwait(false);
                int found = users.ToList().FindIndex(x => x.DisplayName == entity.DisplayName);
                if(found != -1)
                {
                    throw new ConflictException("The given displayName is not unique");
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

        public Task<User> SelectById(string id)
        {
            throw new NotImplementedException();
        }

        public Task Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}