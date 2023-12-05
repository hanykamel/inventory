using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Commands
{
   public class ReEnableExchangeOrderStoreItemsCommand : IRequest<bool>
    {
        public Guid ExchangeOrderId { get; set; }
        public List<Guid> storeItemsIds { get; set; }
    }
}
