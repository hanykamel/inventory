using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Employees:Entity<int>,IActive
    {
        public Employees()
        {
            CommitteeEmployee = new HashSet<CommitteeEmployee>();
            ExchangeOrder = new HashSet<ExchangeOrder>();
            Invoice = new HashSet<Invoice>();
            ExaminationRefundOrder = new HashSet<RefundOrder>();
            EmployeeRefundOrder = new HashSet<RefundOrder>();
        }

        public string Name { get; set; }
        public string CardCode { get; set; }
        public int? DepartmentId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<CommitteeEmployee> CommitteeEmployee { get; set; }
        public virtual ICollection<ExchangeOrder> ExchangeOrder { get; set; }
        public virtual ICollection<RefundOrder> ExaminationRefundOrder { get; set; }

        public virtual ICollection<RefundOrder> EmployeeRefundOrder { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
