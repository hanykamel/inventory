using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Web.Helpers
{
    public class CustomClaimTypes
    {
        public const string FullName = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/fullname";
        public const string Password = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/Password";
        public const string Tenants = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/Tenants";
        public const string SelectedTenant = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/SelectedTenant";
        public const string SelectedTenantName = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/SelectedTenantName";
        public const string IsDelegated = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/IsDelegated";
        public const string DelegationDate = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/DelegationDate";
    }
}
