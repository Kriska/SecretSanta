using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }

        public User(string userName, string displayName, string password)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Password = password;
        }
    }
}