using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Inventory.Service.Entities.TransformationRequest.Commands
{
    public class PrintTransformationFormNo2Command:IRequest<MemoryStream>
    {
        [Required]
        public Guid TransformationId { get; set; }
    }
}
