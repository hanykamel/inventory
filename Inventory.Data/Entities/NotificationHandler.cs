using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class NotificationHandler:Entity<int>,IActive
    {
        public int NotificationTemplateId { get; set; }
        public int HandlerId { get; set; }
        public bool? IsActive { get; set; }
        public virtual NotificationTemplate NotificationTemplate { get; set; }

    }
}
