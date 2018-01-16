using SecretSanta.Entities;
using SecretSanta.CrossDomain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using System.Linq;


namespace SecretSanta.Repository
{
	public class LoginRepository : BaseRepository, IRepository<Login, string>
	{
        public LoginRepository(IDbSettings settings) : base(settings)
        {
        }

        public async Task Delete(string id)
        {
            using (var connection = CreateConnection())
            {
                var logins = await connection.ExecuteAsync(
                @"DELETE FROM logins WHERE idUser= @Id", new { Id = id }).ConfigureAwait(false);
            }

        }

        public async Task<Login> Insert(Login entity)
        {
            using (var connection = CreateConnection())
            {
                var logins = await connection.ExecuteAsync(
                @"INSERT INTO logins (idUser, token)
				VALUES( @IdUser, @AuthnToken)", entity).ConfigureAwait(false);

                return entity;
            }
        }

        public Task<IEnumerable<Login>> SelectAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Login> SelectByUserName(string userName)
        {
			//wont be impelemented
            throw new System.NotImplementedException();
        }

        public async Task<Login> SelectByToken(string token)
        {
            using (var connection = CreateConnection())
            {
                var logins = await connection.QueryAsync<Login>(
				@"SELECT * FROM logins WHERE authnToken = @Token", new { Token = token } ).ConfigureAwait(false);

                return logins.ToList().ElementAt(0); ;
            }
        }
        public async Task<User> GetUserByLogin(Login login)
        {
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User>(
                @"SELECT u.* FROM users u join logins on
				u.Id = logins.idUser WHERE logins.idUser = @IdUser", new { IdUser = login.IdUser }).ConfigureAwait(false);
                if(users.Count() > 0)
                {
                    return users.ToList().ElementAt(0);
                }
                throw new NotFoundException("User not found");
            }
        }
        public Task<Login> Update(Login entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<Login> SelectByGroupName(string groupName)
        {
            //won't be impelemented
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Login>> SelectAllInvitations(int idParticipant)
        {
            //won't be implemented
            throw new System.NotImplementedException();
        }
    }
}