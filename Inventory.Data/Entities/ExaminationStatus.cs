using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class ExaminationStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ExaminationCommitte> ExaminationCommitte { get; set; }

    }
}
