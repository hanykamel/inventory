using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Budget:Entity<int>,IActive
    {
        public Budget()
        {
            Addition = new HashSet<Addition>();
            ExaminationCommitte = new HashSet<ExaminationCommitte>();
            ExchangeOrder = new HashSet<ExchangeOrder>();
            Store = new HashSet<Store>();
            Transformation = new HashSet<Transformation>();
            RefundOrder = new HashSet<RefundOrder>();
            RobbingOrder = new HashSet<RobbingOrder>();


        }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Addition> Addition { get; set; }
        public virtual ICollection<ExaminationCommitte> ExaminationCommitte { get; set; }
        public virtual ICollection<ExchangeOrder> ExchangeOrder { get; set; }
        public virtual ICollection<Store> Store { get; set; }
        public virtual ICollection<Transformation> Transformation { get; set; }
        public virtual ICollection<RefundOrder> RefundOrder { get; set; }

        public virtual ICollection<RobbingOrder> RobbingOrder { get; set; }

    }
}
