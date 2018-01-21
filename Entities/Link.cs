using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSanta.Entities
{
    public class Link
    {
        public int Id { get; set; }
        public string RecieverName { get; set; }
        public string SenderName { get; set; }
        public string GroupName { get; set; }
    }
}