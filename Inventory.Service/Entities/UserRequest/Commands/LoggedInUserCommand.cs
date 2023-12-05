using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UserRequest.Commands
{
   
        public class LoggedInUserCommand : IRequest<LoggedInUserOutput>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
   
}
