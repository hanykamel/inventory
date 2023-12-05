using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.AttachmentVM;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inventory.Data.Entities;
namespace Inventory.Service.Entities.ExchangeOrderRequest.Commands
{
    public class CreateExchangeOrderCommand : IRequest<Guid>
    {
       
        public List<StoreItemVM> items { get; set; }
        [Required]
        public int budgetId { get; set; }
        [Required]
        public int forEmployeeId { get; set; }
        public bool isDirectOrder { get; set; }
        public string notes { get; set; }
        public string directNotes { get; set; }
        
    }
}
