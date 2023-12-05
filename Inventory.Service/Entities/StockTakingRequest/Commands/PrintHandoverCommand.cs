using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Inventory.Service.Entities.StockTakingRequest.Commands
{
    public class PrintHandoverCommand : IRequest<MemoryStream>
    {
        [Required]
        public Guid HandoverId { get; set; }
    }
}
