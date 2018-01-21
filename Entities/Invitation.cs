using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.Entities
{
    public class Invitation
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
    }
}