using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Commands
{
   public class CancelExchangeOrderCommand : IRequest<bool>
    {
        public Guid ExchangeOrderId { get; set; }
    }
}
