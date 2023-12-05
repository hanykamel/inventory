using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class Template_2VM
    {
        public long BaseItemId { get; set; }
        public int Quantity { get; set; }
        public decimal price { get; set; }
        public string BaseItemName { get; set; }
        public string UnitName { get; set; }
        public string Notes { get; set; }
        public string ItemStatus { get; set; }

    }
}
