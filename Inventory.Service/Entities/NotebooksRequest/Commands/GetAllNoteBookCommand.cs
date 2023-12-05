using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.NotebooksRequest.Commands
{
   public class GetAllNoteBookCommand : IRequest<List<NoteBookOutPutCommand>>
    {
    }
}
