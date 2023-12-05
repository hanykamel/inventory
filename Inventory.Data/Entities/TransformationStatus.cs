using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class TransformationStatus
    {
        public TransformationStatus()
        {
            Transformation = new HashSet<Transformation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Transformation> Transformation { get; set; }
    }
}
