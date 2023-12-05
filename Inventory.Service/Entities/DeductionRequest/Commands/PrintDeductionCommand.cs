using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory.Service.Entities.DeductionRequest.Commands
{
    public class PrintDeductionCommand : IRequest<MemoryStream>
    {
        public Guid DeductionId { get; set; }
    }
}
