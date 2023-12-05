using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Inventory.Service.Entities.StockTakingRequest.Commands
{
    public class PrintStockTakingCommand : IRequest<MemoryStream>
    {
        [Required]
        public Guid StockTakingId { get; set; }
    }
}
