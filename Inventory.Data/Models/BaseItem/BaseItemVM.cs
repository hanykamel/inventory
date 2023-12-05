using Inventory.Data.Entities;
using Inventory.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Data.Models.BaseItem
{
    public class InvoiceVM
    {
        public List<Guid> InvoiceStoreItems { get; set; }
    }

    public class BaseItemVM
    {
        public long Id { get; set; }
        public string Name { get; set; }

    }
}
