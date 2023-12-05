using Inventory.Data.Enums;
using System.Collections.Generic;


namespace Inventory.Data.Models.NotificationVM
{
    public class NotificationVM
    {
        public NotificationTemplateEnum notificationTemplateEnum { get; set; }
        public string storeId { get; set; }
        public string ToStoreId { get; set; }
        public string code { get; set; }
        public string Id { get; set; }
        public string FromStore { get; set; }
        public string ToStore { get; set; }
        public string FromStoreAdmin { get; set; }
        public string ToStoreAdmin { get; set; }
        public string TechManager { get; set; }
        public List<string> Users { get; set; }
    }  
}
