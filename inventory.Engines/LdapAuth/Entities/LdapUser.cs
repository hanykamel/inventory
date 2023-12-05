using System;
using System.Collections.Generic;
using System.Text;

namespace inventory.Engines.LdapAuth.Entities
{
    public class LdapUser
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string[] Roles { get; set; }
    }
}
