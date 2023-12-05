using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory.Service.Entities.InvoiceRequest.Commands
{
    public class PrintInvoiceCommand:IRequest<MemoryStream>
    {
        public List<Guid> InvoicesIds { get; set; }
        public  bool? ShowAllInvoiceItems { get; set; }
    }
}
