using Inventory.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.NotebooksRequest.Commands
{
  public  class EditNoteBookCommand : IRequest<bool>
    {
        public long NoteBookId { get; set; }
        public int StoreId { get; set; }
        public int BookNum { get; set; }
        public string NumberOfPages { get; set; }
        //public ItemTypeEnum ItemType { get; set; }
    }
}
