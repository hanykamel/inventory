using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory.Service.Entities.RefundOrderRequest.Commands
{
    public class PrintFromNo9Command : IRequest<MemoryStream>
    {
        public Guid RefundOrderId { get; set; }
    }
}
