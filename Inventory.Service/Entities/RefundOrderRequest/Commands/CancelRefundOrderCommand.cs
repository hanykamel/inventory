using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.RefundOrderRequest.Commands
{
   public class CancelRefundOrderCommand : IRequest<bool>
    {
        public Guid RefundOrderId { get; set; }
    }
}
