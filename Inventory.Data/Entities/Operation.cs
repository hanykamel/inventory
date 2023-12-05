using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Operation
    {
        public Operation()
        {
            Addition = new HashSet<Addition>();
            ExaminationCommitte = new HashSet<ExaminationCommitte>();
            ExchangeOrder = new HashSet<ExchangeOrder>();
            StoreItemHistory = new HashSet<StoreItemHistory>();
            Transformation = new HashSet<Transformation>();
            RefundOrder = new HashSet<RefundOrder>();
            RobbingOrder = new HashSet<RobbingOrder>();
            StockTaking = new HashSet<StockTaking>();
            Deduction = new HashSet<Deduction>();


        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Addition> Addition { get; set; }
        public virtual ICollection<Subtraction> Subtraction { get; set; }

        public virtual ICollection<ExaminationCommitte> ExaminationCommitte { get; set; }
        public virtual ICollection<ExchangeOrder> ExchangeOrder { get; set; }
        public virtual ICollection<StoreItemHistory> StoreItemHistory { get; set; }
        public virtual ICollection<Transformation> Transformation { get; set; }
        public virtual ICollection<OperationAttachmentType> OperationAttachmentType { get; set; }
        public virtual ICollection<RefundOrder> RefundOrder { get; set; }

        public virtual ICollection<RobbingOrder> RobbingOrder { get; set; }
        public virtual ICollection<StockTaking> StockTaking { get; set; }
        public virtual ICollection<Deduction> Deduction { get; set; }


    }
}
