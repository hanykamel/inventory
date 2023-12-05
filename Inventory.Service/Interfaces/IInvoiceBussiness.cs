using Inventory.Data.Entities;
using Inventory.Data.Models.BaseItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IInvoiceBussiness
    {
        Invoice GetInvoiceById(Guid invoiceId);

        IQueryable<Invoice> GetAllInvoice();

        IQueryable<Invoice> GetInvoiceDetails();

        IQueryable<Invoice> PrintInvoicesList();
        bool AddAllInvoice(List<Invoice> AllInvoice);

        Task<List<Guid>> SaveAllInvoice(List<Invoice> AllInvoice);
        List<Invoice> GetMultipleInvoices(List<Guid> invoiceIds);
        Task<bool> Savechange();
        string GetCode(int max);
        string GetCode();
        List<InvoiceStoreItem> GetInvoiceStoreItem(List<Guid> storeItemIds);

         Task<bool> EditeInvoice(List<InvoiceStoreItem> RefundOrderStore);
        IQueryable<BaseItemVM> GetBaseItemsFromInvoice(int empId);

        bool EditInvoiceStoreItemUnderRefund(InvoiceStoreItem invoiceStoreItem);
        bool checkInvoiceStoreItemRefunded(List<Guid> storeItems);
    }
}
