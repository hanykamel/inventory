using inventory.Engines.LdapAuth.Entities;
using Inventory.CrossCutting.Identity;
using Inventory.Web.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.Web.Identity
{
    public class IdentityProvider : IIdentityProvider
    {
        IHttpContextAccessor _httpContextAccessor;
        public IdentityProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x =>
              x.Type == ClaimTypes.Name)?
            .Value;
        }
        public List<string> GetAllowedGroups()
        {
            return new List<string> {UserGroups.StoreKeeper,
                                     UserGroups.TechnicalDepartments,
                                     UserGroups.WarehouseManager,
                                     UserGroups.SystemAdmin,
                                     UserGroups.AssistantTechnicalDepartments};
        }
        public List<string> GetMustHaveTenantGroups()
        {
            return new List<string> {UserGroups.StoreKeeper,
                                     UserGroups.TechnicalDepartments,
                                     UserGroups.AssistantTechnicalDepartments};
        }

    }
}
