using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.TechnicalDepartmentRequest.Commands
{
   public class TechnicalDepartmentOutputCommands
    {
        public int Id { get; set; }

        public string TechnicalDepartmentName { get; set; }

        public bool? IsActive { get; set; }


    }
}
