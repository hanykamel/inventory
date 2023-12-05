using Inventory.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.NotebooksRequest.Commands
{
  public  class AddNoteBookCommand : IRequest<bool>
    {
        public int StoreId { get; set; }
        public string BookNum { get; set; } 
        public int NumberOfPages { get; set; }
       // public ItemTypeEnum ItemType { get; set; }
    }
}
