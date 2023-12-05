using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DelegationRequest.Commands
{
  public  class StopDelegationCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
