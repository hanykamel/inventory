
using inventory.Engines.LdapAuth;
using inventory.Helpers;
using Inventory.CrossCutting.Identity;
using Inventory.Service.Interfaces;
using Inventory.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.Web.Middlewares
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _seconds;

        public RefreshTokenMiddleware(RequestDelegate next, int seconds)
        {
            _next = next;
            _seconds = seconds;
        }

        public async Task InvokeAsync(HttpContext context, ILdapAuthenticationService _authService,IIdentityProvider _identityProvider,IUserBusiness userBusiness)
        {
            //bool isClaimExpired = context.User.IsClaimExpired(_seconds);
            if (context.User.Identity.IsAuthenticated)
            {
                //if (isClaimExpired == true)
                //{
                    //refresh or update claims
                    await Referesh(context, _authService, _identityProvider, userBusiness);
                //}
            }

            await _next(context);
        }
        private async Task<bool> Referesh(HttpContext context, 
            ILdapAuthenticationService _authService,
            IIdentityProvider _identityProvider,IUserBusiness userBusiness)
        {
            try
            {
                var User = context.User;

                var groups = _authService.GetUserGroups(User.Identity.Name);

                var userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, User.Identity.Name),
                            new Claim(CustomClaimTypes.FullName, User.GetFullName()==null?"":User.GetFullName()),

                               new Claim(ClaimTypes.Expiration,DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))

                        };
                //
                if (groups != null && groups.Count > 0)
                {
                    var allowedGroups = _identityProvider.GetAllowedGroups();
                    foreach (var role in groups)
                    {
                        if (groups.Contains(role))
                            userClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                }

                var tenants = User.GetTenants();
                if (tenants != null && tenants.Count > 0)
                {
                    foreach (var tenant in tenants)
                    {
                        userClaims.Add(new Claim(CustomClaimTypes.Tenants,
                            tenant));
                    }
                    //set the default tenant
                    userClaims.Add(new Claim(CustomClaimTypes.SelectedTenant,
                      User.GetSelectedTenant()));

                    //get tenant rest data
                    var _userTenant = userBusiness.GetTenantData(int.Parse(User.GetSelectedTenant()), User.Identity.Name);
                    if (_userTenant != null)
                    {
                        //set the default tenant
                        userClaims.Add(new Claim(CustomClaimTypes.SelectedTenantName,
                            Convert.ToString(_userTenant.Name)));
                        userClaims.Add(new Claim(CustomClaimTypes.IsDelegated,
                           Convert.ToString(_userTenant.IsDelegated)));
                        userClaims.Add(new Claim(CustomClaimTypes.DelegationDate,
                            Convert.ToString(_userTenant.EndDate)));
                    }
                    else
                    {
                        await userBusiness.DeleteUserConnectionId(User.Identity.Name);
                        await context.SignOutAsync();
                        return true;
                    }

                    
                }

                var principal = new ClaimsPrincipal(
                    new ClaimsIdentity(userClaims, _authService.GetType().Name)
                );

                await context.SignInAsync(
                              CookieAuthenticationDefaults.AuthenticationScheme,
                                principal,
                                new AuthenticationProperties
                                {
                                    IsPersistent = true,
                                    AllowRefresh = true,
                                }
                            );

                return true;
            }
            catch(Exception ex)
            {

                return true;
            }
        }
    }
}
