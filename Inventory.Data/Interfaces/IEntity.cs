using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
        string  CreatedBy { get; set; }
        DateTime CreationDate { get; set; }
        string ModifiedBy { get; set; }
        DateTime? ModificationDate { get; set; }
        
       
    }
}
