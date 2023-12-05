using Inventory.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.NotebooksRequest.Commands
{
  public  class NoteBookOutPutCommand
    {
        public long NoteBook { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string BookNum { get; set; }
        public string NumberOfPages { get; set; }
        //public ItemTypeEnum ItemType { get; set; }
    }
}
