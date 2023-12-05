using inventory.Engines.LdapAuth.Entities;

using Inventory.Data.Models.TenantVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UserRequest.Commands
{
    public class LoggedInUserOutput
    {
        public LdapUser User { get; set; }
        public List<TenantVM> Tenants { get; set; }
    }
}
