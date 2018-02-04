using SecretSanta.Entities;
using SecretSanta.CrossDomain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using System.Linq;


namespace SecretSanta.Repository
{
	public class LoginRepository : BaseRepository, ILoginRepository
	{
        public LoginRepository(IDbSettings settings) : base(settings)
        {
        }
        public async Task<Login> Insert(Login entity)
        {
            using (var connection = CreateConnection())
            {
               await connection.ExecuteAsync( @"INSERT INTO logins (userName, authnToken)
                                                VALUES( @UserName, @AuthnToken)", entity).ConfigureAwait(false);

                return entity;
            }
        }
        public Task<Login> Update(Login entity)
        {
            throw new System.NotImplementedException();
        }
        public async Task<int> Delete(string userName)
        {
            using (var connection = CreateConnection())
            {
                var afected = await connection.ExecuteAsync(
                @"DELETE FROM logins WHERE userName= @UserName", new { UserName = userName }).ConfigureAwait(false);
                return afected;
            }
        }
        public Task<IEnumerable<Login>> SelectAll()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Login> SelectByKey(string authnToken)
        {
            using (var connection = CreateConnection())
            {
                var logins = await connection.QueryAsync<Login>(
                @"SELECT * FROM logins WHERE authnToken = @AuthnToken", new { AuthnToken = authnToken }).ConfigureAwait(false);
                if (logins.Count() <= 0)
                {
                    return null;
                }
                return logins.ElementAt(0); ;
            }
        }        
        public async Task<Login> SelectByUserName(string userName)
        {
            using (var connection = CreateConnection())
            {
                var logins = await connection.QueryAsync<Login>(
                @"SELECT * FROM logins WHERE userName = @UserName", new { UserName = userName }).ConfigureAwait(false);
                if (logins.Count() <= 0)
                {
                    return null;
                }
                return logins.ElementAt(0); ;
            }
        }
    }
}