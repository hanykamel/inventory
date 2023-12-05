using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class Notification:Entity<Guid>,IActive
    {
        public Notification()
        {
            NotificationValues = new HashSet<NotificationValues>();
            UserNotification = new HashSet<UserNotification>();
        }
        public string Body { get; set; }
        public bool? IsActive { get; set; }
        public int NotificationTemplateId { get; set; }
        public virtual ICollection<NotificationValues> NotificationValues { get; set; }
        public virtual ICollection<UserNotification> UserNotification { get; set; }
        public virtual NotificationTemplate NotificationTemplate { get; set; }
        
    }
}
