using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory.Service.Entities.AdditionRequest.Commands
{
    public class PrintFormNo8Command:IRequest<MemoryStream>
    {
        public Guid AdditionId { get; set; }
    }
}
