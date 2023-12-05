using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.DepartmentRequest.Commands
{
   public class DepartmentOutputCommand
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public bool? IsActive { get; set; }
    }
}
