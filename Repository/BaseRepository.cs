using System.Data.SqlClient;
using SecretSanta.CrossDomain;

namespace SecretSanta.Repository
{
    public class BaseRepository
    {
        private string _connectionString;

        public BaseRepository(IDbSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
