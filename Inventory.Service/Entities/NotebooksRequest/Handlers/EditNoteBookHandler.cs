using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using Inventory.Service.Entities.NotebooksRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;

namespace Inventory.Service.Entities.NotebooksRequest.Handlers
{
    public class EditNoteBookHandler : IRequestHandler<EditNoteBookCommand, bool>
    {
        private readonly INoteBooksBussiness _INoteBookBussiness;
        public EditNoteBookHandler(INoteBooksBussiness INoteBookBussiness)
        {
            _INoteBookBussiness = INoteBookBussiness;
        }
        public Task<bool> Handle(EditNoteBookCommand request, CancellationToken cancellationToken)
        {
            Book entity = new Book();
            entity.Id = request.NoteBookId;
            entity.StoreId = request.StoreId;
            entity.PageCount = request.BookNum;
            //entity. = request.NumberOfPages;
            return Task.FromResult(_INoteBookBussiness.UpdateBook(entity));
        }
    }
}
