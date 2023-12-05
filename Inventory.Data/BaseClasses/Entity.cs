using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class Entity<T> : IEntity<T> where T : struct
    {
        public Entity()
        {
            CreationDate = DateTime.Now;
        }
        public T Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        
    }
}
