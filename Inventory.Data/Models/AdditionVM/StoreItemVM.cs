using Inventory.Data.Entities;
using Inventory.Data.Models.AttachmentVM;
using Inventory.Data.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Data.Models.AdditionVM
{
    public class StoreItemVM
    {
        public Guid AdditionId { get; set; }
        public long BaseItemId { get; set; }
        public string Code { get; set; }
        public Guid Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public int? ContractNumber { get; set; }
        public int? PageNumber { get; set; }
        public decimal Price { get; set; }
        public int? StoreId { get; set; }
        public int Serial { get; set; }
        public int StoreItemStatusId { get; set; }
        public int CurrentItemStatusId { get; set; }
        public string Notes { get; set; }
        public int? CurrencyId { get; set; }
        public String CurrencyName { get; set; }
        public BaseItemVm BaseItem { get; set; }
        public LookupVM<int> StoreItemStatusValue { get; set; }

    }
}
