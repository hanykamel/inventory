using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.LocationRequest.Commands
{
   public class LocationOutputCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
