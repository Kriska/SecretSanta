namespace SecretSanta.Entities
{
    public class Login
    {
        public string AuthnToken { get; set; }
        public string UserName { get; set; }
        public Login(string userName, string authnToken)
        {
            this.UserName = userName;
            this.AuthnToken = authnToken;
        }

        public Login()
        {
        }
    }
}