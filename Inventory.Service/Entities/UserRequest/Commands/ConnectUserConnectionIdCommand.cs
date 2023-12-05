using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UserRequest.Commands
{

    public class ConnectUserConnectionIdCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }

}
