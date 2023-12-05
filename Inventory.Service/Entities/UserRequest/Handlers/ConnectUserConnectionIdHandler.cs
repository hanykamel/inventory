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
        public class ConnectUserConnectionIdHandler : IRequestHandler<ConnectUserConnectionIdCommand, string>
        {
            private readonly IUserBusiness _userBussiness;
            public ConnectUserConnectionIdHandler(IUserBusiness userBusiness)
            {
                _userBussiness = userBusiness;
            }
            public async Task<string> Handle(ConnectUserConnectionIdCommand request, CancellationToken cancellationToken)
            {
                return await _userBussiness.ConnectUserConnectionId(request.Username, request.ConnectionId);
            }
        }
    }

}
