using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class NotificationTemplate: Entity<int>,IActive
    {
        public NotificationTemplate()
        {
            NotificationHandler = new HashSet<NotificationHandler>();
            Notification = new HashSet<Notification>();

        }

        public string Body { get; set; }
        public string URL { get; set; }
        public virtual ICollection<NotificationHandler> NotificationHandler { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }

        public bool? IsActive { get; set; }

    }
}
