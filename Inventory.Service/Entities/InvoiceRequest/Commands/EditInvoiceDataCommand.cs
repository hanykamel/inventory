using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.InvoiceRequest.Commands
{
  public  class EditInvoiceDataCommand: IRequest<bool>
    {
        public Guid Id { get; set; }
        public int? DepartmentId { get; set; }
        public String Date { get; set; }
        public int ReceivedEmployeeId { get; set; }
        public int LocationId { get; set; }
    }
}
