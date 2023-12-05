using MediatR;
using System;
using System.IO;

namespace Inventory.Service.Entities.ExecutionOrderRequest.Commands
{
    public class PrintExecutionOrderCommand : IRequest<MemoryStream>
    {
        public Guid ExecutionOrderId { get; set; }
    }
}
