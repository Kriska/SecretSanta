using System.Configuration;

namespace SecretSanta.CrossDomain
{
    public class Settings : IDbSettings
    {
        public string ConnectionString => ConfigurationManager.AppSettings["Db.ConnectionString"];
    }
}