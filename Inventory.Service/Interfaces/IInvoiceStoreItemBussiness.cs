using Inventory.Data.Entities;
using System.Collections.Generic;


namespace Inventory.Service.Interfaces
{
    public interface IInvoiceStoreItemBussiness
    {
        IEnumerable<InvoiceStoreItem> GetAllInvoiceStoreItems();
        bool CancelInvoiceStoreItemUnderRefund(ICollection<RefundOrderStoreItem> refundOrderStoreItems);
    }
}
