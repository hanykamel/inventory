using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Service.Implementation
{
    public class InvoiceStoreItemBussiness : IInvoiceStoreItemBussiness
    {
        private readonly IRepository<InvoiceStoreItem, Guid> _invoiceStoreItemRepository;
        public InvoiceStoreItemBussiness(
            IRepository<InvoiceStoreItem, Guid> invoiceStoreItemRepository
            )
        {
            _invoiceStoreItemRepository = invoiceStoreItemRepository;
        }

        public IEnumerable<InvoiceStoreItem> GetAllInvoiceStoreItems()
        {
            var InvoiceStoreItemList = _invoiceStoreItemRepository.GetAll();
            return InvoiceStoreItemList;
        }
        public bool CancelInvoiceStoreItemUnderRefund(ICollection<RefundOrderStoreItem> refundOrderStoreItems)
        {
            foreach (var item in refundOrderStoreItems)
            {
                var invoiceItem = _invoiceStoreItemRepository.GetAll().Where(i => i.StoreItemId == item.StoreItemId).FirstOrDefault();
                invoiceItem.UnderRefunded = null;
                _invoiceStoreItemRepository.Update(invoiceItem);
            }
            return true;
        }


    }


}
