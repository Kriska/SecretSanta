using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.Entities
{
    public class Group
    {
        public string GroupName { get; set; }
        public string AdminName { get; set; }

        public Group(string groupName, string adminName)
        {
            this.GroupName = groupName;
            this.AdminName = adminName;
        }

        public Group()
        {
        }
    }
}