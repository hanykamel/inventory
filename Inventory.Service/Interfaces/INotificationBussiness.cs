using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface INotificationBussiness
    {
        Task SendNotification(NotificationVM Notification);
        IQueryable<UserNotification> GetAll();
        //IQueryable<UserNotification> GetAllUserNotifications();
        Task<bool> MarkAsRead(Guid id);
    }
}
