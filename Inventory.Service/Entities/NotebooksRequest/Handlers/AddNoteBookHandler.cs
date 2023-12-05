using Inventory.Data.Entities;
using Inventory.Service.Entities.NotebooksRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.NotebooksRequest.Handlers
{
    public class AddNoteBookHandler : IRequestHandler<AddNoteBookCommand, bool>
    {

        private readonly INoteBooksBussiness _INoteBookBussiness;
        public AddNoteBookHandler(INoteBooksBussiness INoteBookBussiness)
        {
            _INoteBookBussiness = INoteBookBussiness;
        }
        public Task<bool> Handle(AddNoteBookCommand request, CancellationToken cancellationToken)
        {
            Book entity = new Book();
            entity.StoreId = request.StoreId;
            entity.PageCount = request.NumberOfPages;
            return Task.FromResult(_INoteBookBussiness.AddNewBook(entity));
        }
    }
}
