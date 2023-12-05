using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class NotificationValues:Entity<int>,IActive
    {
        public Guid NotificationId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool? IsActive { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
