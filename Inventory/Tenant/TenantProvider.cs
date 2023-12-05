using Inventory.CrossCutting.Tenant;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Web.Tenant
{
    public class TenantProvider : ITenantProvider
    {
        IHttpContextAccessor _httpContextAccessor; 
        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    
        public int GetTenant()
        {
            var _tenant = 0;

            int.TryParse(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x =>
              x.Type == CustomClaimTypes.SelectedTenant)?
            .Value, out _tenant);

            return _tenant;

        }
        public string GetTenantName()
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x =>
              x.Type == CustomClaimTypes.SelectedTenantName)?
            .Value;
        }
        public bool ChcekIsDelegated()
        {
            var isDelegated = false;

            bool.TryParse(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x =>
              x.Type == CustomClaimTypes.IsDelegated)?
            .Value, out isDelegated);

            return isDelegated;

        }
    }
}
