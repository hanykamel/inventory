using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inventory.Data.Entities;
namespace Inventory.Service.Entities.AdditionRequest.Commands
{
    public class GenerateStoreItemsBarCodeCommand : IRequest<List<StoreItemVM>>
    {
        public Guid AdditionId { get; set; }
    }
}
