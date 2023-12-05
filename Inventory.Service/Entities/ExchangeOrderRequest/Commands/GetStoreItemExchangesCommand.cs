using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inventory.Data.Entities;
using Inventory.Data.Models.ExchangeOrderVM;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Commands
{
    public class GetStoreItemExchangesCommand : IRequest<List<StoreItemExchangesVM>>
    {
        public Guid id { get; set; }
    }
}
