using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.ItemCategoryRequest.Commands
{
   public class ItemCategoryOutPutCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
