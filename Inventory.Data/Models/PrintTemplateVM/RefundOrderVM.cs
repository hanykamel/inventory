using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class RefundOrderVM
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int? RefundOrderStatusId { get; set; }
        public RefundOrderStatusVM RefundOrderStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public int TenantId { get; set; }

    }

    public class RefundOrderStatusVM
    {
        public string Name { get; set; }
    }
}
