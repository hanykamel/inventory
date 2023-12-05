using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.CrossCutting.Tenant
{
    public interface ITenantProvider
    {
        int GetTenant();
        string GetTenantName();
        bool ChcekIsDelegated();

    }
}
