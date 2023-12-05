using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class Form2PrintObjectVM
    {
        public string Currency { get; set; }
        public List<BaseItemStoreItemVM> value { get; set; }
    }
}
