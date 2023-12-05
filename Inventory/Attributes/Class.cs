using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.Web.Attributes
{
    
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(params string[] claimValue) : base(typeof(ClaimRequirementFilter))
        {

            Arguments = new object[] {claimValue};
         
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly string[] _claims;
        public ClaimRequirementFilter( string[] claims)
        {
            _claims = claims;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && 
            _claims.Contains(c.Value));
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
//[ClaimRequirement(UserGroups.StoreKeeper, UserGroups.TechnicalDepartments)] 