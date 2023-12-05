using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class Currency : Entity<int>, IActive
    {
        public Currency()
        {
            ExaminationCommitte = new HashSet<ExaminationCommitte>();
            StoreItem = new HashSet<StoreItem>();
            RobbedStoreItem = new HashSet<RobbedStoreItem>();


        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
        public virtual ICollection<ExaminationCommitte> ExaminationCommitte { get; set; }
        public virtual ICollection<StoreItem> StoreItem { get; set; }
        public virtual ICollection<RobbedStoreItem> RobbedStoreItem { get; set; }

    }
}
