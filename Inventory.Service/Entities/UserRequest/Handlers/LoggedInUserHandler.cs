using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UserRequest.Handlers
{
    using global::Inventory.Service.Entities.UserRequest.Commands;
    using global::Inventory.Service.Interfaces;
    using inventory.Engines.LdapAuth;
    using inventory.Engines.LdapAuth.Entities;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    namespace Inventory.Service.Handlers
    {
        public class LoggedInUserHandler : IRequestHandler<LoggedInUserCommand, LoggedInUserOutput>
        {
            private readonly IUserBusiness _userBussiness;
            private readonly ILdapAuthenticationService _ldapAuthenticationService;
            public LoggedInUserHandler(IUserBusiness userBusiness,
                ILdapAuthenticationService ldapAuthenticationService)
            {
                _userBussiness = userBusiness;
                _ldapAuthenticationService = ldapAuthenticationService;
            }
            public Task<LoggedInUserOutput> Handle(LoggedInUserCommand request, CancellationToken cancellationToken)
            {
                //get the user information from active directory
                var user=_ldapAuthenticationService.Login(request.Username, request.Password);
                if (user == null)
                    throw new Exception("Can't login to Active Directory,returned user is empty") ;
                //get user tenants from DB
                var tenants=_userBussiness.GetUserTenants(user.Username);

                return Task.FromResult(new LoggedInUserOutput
                { User = user, Tenants = tenants });
               
            }
        }
    }

}
