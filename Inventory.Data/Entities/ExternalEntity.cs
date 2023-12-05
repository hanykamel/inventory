using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ExternalEntity:Entity<int>,IActive
    {
        public ExternalEntity()
        {
            ExaminationCommitte = new HashSet<ExaminationCommitte>();
            Transformation = new HashSet<Transformation>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<ExaminationCommitte> ExaminationCommitte { get; set; }
        public virtual ICollection<Transformation> Transformation { get; set; }
    }
}
