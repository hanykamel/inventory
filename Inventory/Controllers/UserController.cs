using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Inventory.Web.Helpers;
using inventory.Engines.LdapAuth;
using Inventory.Service.Entities.UserRequest.Commands;
using inventory.Helpers;
using Inventory.CrossCutting.Identity;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.Service.Interfaces;

namespace Inventory.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILdapAuthenticationService _ldapAuthenticationService;
        private readonly IIdentityProvider _identityProvider;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IUserBusiness _userBusiness;
        public UserController(IMediator mediator,
            ILdapAuthenticationService ldapAuthenticationService,
            IIdentityProvider identityProvider,
            IStringLocalizer<SharedResource> localizer,
            IUserBusiness userBusiness)
        {
            _mediator = mediator;
            _ldapAuthenticationService = ldapAuthenticationService;
            _identityProvider = identityProvider;
            _localizer = localizer;
            _userBusiness = userBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region Login
        //[Authorize(Policy  = "ADRoleOnly")]

        // [ValidateActionParameters]
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody]LoggedInUserCommand loggedInUser)
        {
            var mockTenant = new Data.Models.TenantVM.TenantVM { Id = 43, IsDelegated = false, Name = "tanant", EndDate = DateTime.Now };

            //var user = _mediator.Send(loggedInUser).Result;
            var user = new LoggedInUserOutput
            {
                User = new inventory.Engines.LdapAuth.Entities.LdapUser
                { FullName = "name", Username = "userName", Roles = new string[] { "StoreKeeper" } },
                Tenants = new List<Data.Models.TenantVM.TenantVM> {
                    mockTenant
            }
            };
            if (user != null)
            {
                var userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.User.Username),
                            new Claim(CustomClaimTypes.FullName, user.User.FullName),
                            new Claim(ClaimTypes.Expiration,DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                        };

                // Roles
                //check if groups contains our groups
                var allowedGroups = _identityProvider.GetAllowedGroups();
                var groups = user.User.Roles;
                if (groups != null && groups.Count() > 0)
                {
                    foreach (var role in groups)
                    {
                        if (allowedGroups.Contains(role))
                            userClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    if (userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role) == null)
                        throw new UnAuthorizedException(_localizer["UnAuthorizedException"]);
                }
                else
                    throw new UnAuthorizedException(_localizer["UnAuthorizedException"]);


                // tenants
                if (user.Tenants != null && user.Tenants.Count > 0)
                {
                    foreach (var tenant in user.Tenants.Select(x => x.Id))
                    {
                        userClaims.Add(new Claim(CustomClaimTypes.Tenants,
                            Convert.ToString(tenant)));
                    }
                    //set the default tenant
                    userClaims.Add(new Claim(CustomClaimTypes.SelectedTenant,
                        Convert.ToString(user.Tenants.Select(x => x.Id).FirstOrDefault())));
                    userClaims.Add(new Claim(CustomClaimTypes.SelectedTenantName,
                       Convert.ToString(user.Tenants.Select(x => x.Name).FirstOrDefault())));
                    userClaims.Add(new Claim(CustomClaimTypes.IsDelegated,
                         Convert.ToString(user.Tenants.Select(x => x.IsDelegated).FirstOrDefault())));
                    userClaims.Add(new Claim(CustomClaimTypes.DelegationDate,
                       Convert.ToString(user.Tenants.Select(x => x.EndDate).FirstOrDefault())));
                }
                else
                {
                    var tenantGroups = _identityProvider.GetMustHaveTenantGroups();

                    if (tenantGroups.Contains(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value ))
                        throw new UnAuthorizedException(_localizer["UserHasNoTenantException"]);

                }


                var principal = new ClaimsPrincipal(
                    new ClaimsIdentity(userClaims, _ldapAuthenticationService.GetType().Name)
                );

                await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                    }
                );

                return await Task.FromResult(Ok(new
                {
                    Success = true,
                    UserName = user.User.Username,
                    Tenants = user.Tenants,
                    FullName = user.User.FullName,
                    Group = user.User.Roles[0],
                    
                }));
            }
            else
                throw new InvalidUserNameOrPasswordException(_localizer["InvalidUserNameOrPasswordException"]);
        }
        #endregion
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            var userName = HttpContext.User.Identity.Name;
            bool deleted = await _userBusiness.DeleteUserConnectionId(userName);
            if (!deleted)
            {
                throw new InternalServerError(_localizer["InternalServerError"]);
            }
            await HttpContext.SignOutAsync();
            return NoContent();
        }


        [Authorize(Policy = InventoryAuthorizationPolicy.ChangeTenant)]
        [HttpPost("[action]")]
        public async Task<ActionResult> ChangeTenant(string TenantId)
        {
            var User = HttpContext.User;
            var groups = _ldapAuthenticationService.GetUserGroups(User.Identity.Name);
            var userClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, User.Identity.Name),
                            new Claim(CustomClaimTypes.FullName, User.GetFullName()),

                            new Claim(ClaimTypes.Expiration,DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")),
                        };


            foreach (var role in groups)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tenants = User.GetTenants();
            if (tenants != null && tenants.Count > 0)
            {
                if (!tenants.Contains(TenantId))
                    throw new NotValidTenant(_localizer["NotValidTenant"]);

                foreach (var tenant in tenants)
                {
                    userClaims.Add(new Claim(CustomClaimTypes.Tenants,
                        tenant));
                }
                //get tenant rest data
                var _userTenant=_userBusiness.GetTenantData(int.Parse(TenantId), User.Identity.Name);
                if (_userTenant != null)
                {
                    //set the default tenant
                    userClaims.Add(new Claim(CustomClaimTypes.SelectedTenant,
                      Convert.ToString( _userTenant.Id)));
                    userClaims.Add(new Claim(CustomClaimTypes.SelectedTenantName,
                        Convert.ToString( _userTenant.Name)));
                    userClaims.Add(new Claim(CustomClaimTypes.IsDelegated,
                       Convert.ToString(_userTenant.IsDelegated) ));
                    userClaims.Add(new Claim(CustomClaimTypes.DelegationDate,
                        Convert.ToString(_userTenant.EndDate)));
                }
                else
                    throw new NotValidTenant(_localizer["NotValidTenant"]);
            }

            var principal = new ClaimsPrincipal(
            new ClaimsIdentity(userClaims, _ldapAuthenticationService.GetType().Name));


            
            await HttpContext.SignInAsync(
                          CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                AllowRefresh = true,
                            }
                        );

            return await Task.FromResult(Ok
                (new { Success = true, SelectedTenant = TenantId }));
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet("[action]")]
        public IActionResult SearchTechnicanADUsers([FromQuery]string term)
        {
            if (string.IsNullOrEmpty(term))
                throw new NullSearchValueException(_localizer["NullSearchValueException"]);
            var result = _ldapAuthenticationService.GetTechnicanUserNames(term.Trim());

            return Json(new { Result = result });
        }
        [Authorize(Policy =InventoryAuthorizationPolicy.AllValidRoles)]
        [HttpGet("[action]")]
        public IActionResult SearchAssistantTechnicanADUsers([FromQuery]string term)
        {
            if (string.IsNullOrEmpty(term))
                throw new NullSearchValueException(_localizer["NullSearchValueException"]);
            var result = _ldapAuthenticationService.GetAssistantTechnicanUserNames(term.Trim());

            return Json(new { Result = result });
        }
        [Authorize(Policy = InventoryAuthorizationPolicy.ViewADStoreAdmins)]
        [HttpGet("[action]")]
        public IActionResult SearchStoreKeeperADUsers([FromQuery]string term)
        {
            if (string.IsNullOrEmpty(term))
              throw new NullSearchValueException(_localizer["NullSearchValueException"]);
            var result = _ldapAuthenticationService.GetStoreKeeperUserNames(term.Trim());

            return Json(new { Result = result });
        }


        [HttpGet("[action]")]
        public IActionResult GetUserOriginalTenants([FromQuery]string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new NullSearchValueException(_localizer["NullSearchValueException"]);
            var result = _userBusiness.GetUserOriginalTenants(userName);

            return Json(new { Result = result });
        }



        [HttpPost("[action]")]
        public IActionResult ConnectUserWithConnectionId([FromQuery]ConnectUserConnectionIdCommand connectUserConnectionIdCommand) => Ok(new { result = _mediator.Send(connectUserConnectionIdCommand).Result });

    }
}