using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public int IdAdmin { get; set; }
        public int IdReceiver { get; set; }
        public int IdParticipant { get; set; }
        public string GroupName { get; set; }

        public Group(int id, int idAdmin,int idParticipant, int idReceiver, string groupName)
        {
            this.Id = id;
            this.IdAdmin = idAdmin;
            this.IdReceiver = idReceiver;
            this.IdParticipant = idParticipant;
            this.GroupName = groupName;
        }

        public Group()
        {
        }
    }
}