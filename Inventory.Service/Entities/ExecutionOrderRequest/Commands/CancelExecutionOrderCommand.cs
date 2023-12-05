using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Commands
{
   public class CancelExecutionOrderCommand : IRequest<bool>
    {
        public Guid ExecutionOrderId { get; set; }
    }
}


