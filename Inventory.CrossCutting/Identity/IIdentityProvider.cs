using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.CrossCutting.Identity
{
    public interface IIdentityProvider
    {
        string GetUserName();
        List<string> GetAllowedGroups();
        List<string> GetMustHaveTenantGroups();
    }
}
