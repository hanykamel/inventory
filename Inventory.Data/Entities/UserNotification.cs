using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class UserNotification:Entity<Guid>,IActive
    {
        public string UserName { get; set; }
        public Guid NotificationId { get; set; }
        public bool IsRead { get; set; }
        public string URL { get; set; }
        public Notification Notification { get; set; }
        public bool? IsActive { get; set; }
    }
}
