using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AttachmentVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inventory.Data.Models.NotificationVM
{
    public class NotificationUserVM
    {
        public string User { get; set; }
        public string URL { get; set; }
    }  
}
