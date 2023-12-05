using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.InvoiceRequest.Commands
{
   public class AddInvoiceCommand : IRequest<List<Guid>>
    {
        public string Code { get; set; }
        public int? DepartmentId { get; set; }
        //public DateTime Date { get; set; }
        public String Date { get; set; }
        public int ReceivedEmployeeId { get; set; }
        public Guid ExchangeOrderId { get; set; }
        public int LocationId { get; set; }
        public bool checkInvoice { get; set; }
        public bool statusExchangeOrder { get; set; }
        public List<Guid> InvoiceStoreItem { get; set; }
    }
}
