using SecretSanta.Entities;

namespace SecretSanta.Models
{
    public class UserProfile
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public UserProfile(User user)
        {
            this.UserName = user.UserName;
            this.DisplayName = user.DisplayName;
        }
    }
}