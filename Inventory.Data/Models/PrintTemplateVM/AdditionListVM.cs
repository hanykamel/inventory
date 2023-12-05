using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class AdditionListVM
    {
        public string Code { get; set; }
        public int? AdditionNumber { get; set; }
        public DateTime Date { get; set; }
        public int TenantId { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
