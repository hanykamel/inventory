using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.UnitRequest.Commands
{
   public class UnitOutputCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
