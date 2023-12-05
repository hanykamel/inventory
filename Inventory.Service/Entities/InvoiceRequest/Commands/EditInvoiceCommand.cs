using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.InvoiceRequest.Commands
{
   public class EditInvoiceCommand: IRequest<bool>
    {
        public Guid RefundOrderId { get; set; }
        public List<storeItemList> storeItem { get; set; }
        public InvoiceVm Invoice { get; set; }
    }

    public class storeItemList
    {
        public Guid nvoiceStoreId { get; set; }
        public Guid storeitemId { get; set; }
        public int storeStatus { get; set; }
    }
    public class InvoiceVm
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
    }

}
