using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.Inquiry
{
    public class StagnantModelVM
    {
        public DateTime DateFrom { get; set; }
        public List<StagnantBaseItemVM> StoreItems  { get; set; }
    }

    public class StagnantStoreItemsVM
    {
        public Guid Id { get; set; }
        public String Code { get; set; }
        public String StoreItem { get; set; }
        public String Unit { get; set; }
        public String StoreItemStatus { get; set; }

    }

    public class StagnantBaseItemVM
    {
        public int index { get; set; }
        public int TenantId { get; set; }
        public long BaseItemId { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public String Unit { get; set; }
        public int StoreItemsCount { get; set; }
        public int PageNumber { get; set; }
        public string ContractNumber { get; set; }
        public int BookNumber { get; set; }
        public long BookId { get; set; }

        public int? UnitId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastExchangeOrderDate { get; set; }
        public DateTime LastAdditionDate { get; set; }

    }
    public class StagnantItemsVM
    {
        public Guid Id { get; set; }
        public long BookId { get; set; }
        public long BaseItemId { get; set; }
        public int? UnitId { get; set; }
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public string ContractNumber { get; set; }
        public String BaseItemName { get; set; }
        public String Code { get; set; }
        public String UnitName { get; set; }
        public DateTime? ExchangeOrderDate { get; set; }
        public int TenantId { get; set; }
        public int Count { get; set; }

        public DateTime LastAdditionDate { get; set; }

    }
    public class AllStoreItemsForStagnantInquiriesVM
    {
        public Guid Id { get; set; }
        public long BookId { get; set; }
        public long BaseItemId { get; set; }
        public int? UnitId { get; set; }
        public int PageNumber { get; set; }
        public string ContractNumber { get; set; }
        public String BaseItemName { get; set; }
        public int TenantId { get; set; }
        public DateTime? LastExchangeOrderDate { get; set; }
        public DateTime? LastRefundOrderDate { get; set; }
    }
    public class StagnantStoreItemVM
    {
        public long BaseItemId { get; set; }
        public long BookId { get; set; }
        public int PageNumber { get; set; }
        public string ContractNumber { get; set; }

        public String Code { get; set; }
        public String Name { get; set; }
        public String UnitName { get; set; }
        public String StoreItemStatusName { get; set; }
        public int? UnitId { get; set; }
        public decimal Price { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
