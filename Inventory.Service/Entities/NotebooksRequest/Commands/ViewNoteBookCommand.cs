using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.NotebooksRequest.Commands
{
   public class ViewNoteBookCommand: IRequest<NoteBookOutPutCommand>
    {
        public long Id { get; set; }
    }
}
