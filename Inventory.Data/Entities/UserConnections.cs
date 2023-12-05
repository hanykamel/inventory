using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class UserConnection:Entity<Guid>,IActive
    {
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public string Device { get; set; }
        public bool? IsActive { get; set; }

    }
}
