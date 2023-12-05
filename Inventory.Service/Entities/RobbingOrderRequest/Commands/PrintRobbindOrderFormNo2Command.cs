using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Inventory.Service.Entities.RobbingOrderRequest.Commands
{
    public class PrintRobbingOrderFormNo2Command:IRequest<MemoryStream>
    {
        [Required]
        public Guid RobbingOrderId { get; set; }
    }
}
