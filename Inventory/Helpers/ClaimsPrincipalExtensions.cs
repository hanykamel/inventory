using Inventory.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace inventory.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static Claim GetClaim(this ClaimsPrincipal user, string claimType)
        {
            return user.Claims
                .SingleOrDefault(c => c.Type == claimType);
        }
        public static List<Claim> GetClaimList(this ClaimsPrincipal user, string claimType)
        {
            return user.Claims.Where
                (c => c.Type == claimType)?.ToList();
        }
        public static string GetFullName(this ClaimsPrincipal user)
        {
            var claim = GetClaim(user, CustomClaimTypes.FullName);

            return claim?.Value;
        }

        public static string GetSelectedTenant(this ClaimsPrincipal user)
        {
            var claim = GetClaim(user, CustomClaimTypes.SelectedTenant);

            return claim?.Value;
        }
        public static string GetSelectedTenantName(this ClaimsPrincipal user)
        {
            var claim = GetClaim(user, CustomClaimTypes.SelectedTenantName);

            return claim?.Value;
        }
        public static string CheckIsDelegated(this ClaimsPrincipal user)
        {
            var claim = GetClaim(user, CustomClaimTypes.IsDelegated);

            return claim?.Value;
        }
        public static List<string> GetTenants(this ClaimsPrincipal user)
        {
            var claimList = GetClaimList(user, CustomClaimTypes.Tenants);
            if(claimList !=null && claimList.Count>0)
            {
                List<string> tenantList = new List<string>();
                foreach (var claim in claimList)
                    tenantList.Add(claim?.Value);

                return tenantList;
            }
            return null;
           
        }
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var claim = GetClaim(user, ClaimTypes.Email);

            return claim?.Value;
        }

        public static string GetGroups(this ClaimsPrincipal user)
        {
            var claim = GetClaim(user, ClaimTypes.Role);

            return claim?.Value;
        }

        public static string GetPassword(this ClaimsPrincipal user)
        {
            var claim = GetClaim(user, CustomClaimTypes.Password);

            return claim?.Value;
        }

        public static bool IsClaimExpired(this ClaimsPrincipal user, int seconds)
        {

            var IssuedUtc = Convert.ToDateTime(GetClaim(user, ClaimTypes.Expiration)?.Value);

            //DateTime IssuedUtc =
            //DateTime.ParseExact(GetClaim(user, ClaimTypes.Expiration)?.Value,
            //"yyyy-MM-dd HH:mm:ss",
            // CultureInfo.InvariantCulture);
            if (IssuedUtc == new DateTime()
                || IssuedUtc.AddSeconds(seconds) <
                Convert.ToDateTime(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")))
            {
                return true;
            }
            return false;
        }

    }
}