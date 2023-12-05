using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.RobbingOrderRequest.Commands
{
   public class CancelRobbingOrderCommand : IRequest<bool>
    {
        public Guid RobbingOrderId { get; set; }
    }
}

