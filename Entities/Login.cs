namespace SecretSanta.Entities
{
    public class Login
    {
        public int Id { get; set; } 
        public int IdUser { get; set; }
        public string AuthnToken { get; set; }
        public Login(int id, int idUser, string authnToken)
        {
            this.Id = id;
            this.IdUser = idUser;
            this.AuthnToken = authnToken;
        }
        public Login(int IdUser)
        {
            this.IdUser = IdUser;
            this.AuthnToken = "token"; //to be distributed by TokenManager
        }

    }
}