using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class StoreType
    {
        public StoreType()
        {
            Store = new HashSet<Store>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Store> Store { get; set; }
    }
}
