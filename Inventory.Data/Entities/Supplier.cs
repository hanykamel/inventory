using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Supplier:Entity<int>,IActive
    {
        public Supplier()
        {
            ExaminationCommitte = new HashSet<ExaminationCommitte>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<ExaminationCommitte> ExaminationCommitte { get; set; }
    }
}
