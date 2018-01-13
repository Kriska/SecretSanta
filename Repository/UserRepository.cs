using SecretSanta.CrossDomain;
using SecretSanta.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper; 

namespace SecretSanta.Repository
{
    public class UserRepository : BaseRepository, IRepository<User, String>
    {
        public UserRepository(IDbSettings settings) : base(settings)
        {
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(User entity)
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