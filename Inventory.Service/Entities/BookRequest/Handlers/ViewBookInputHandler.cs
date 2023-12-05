using Inventory.Data.Enums;
using Inventory.Service.Entities.BookRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.BookRequest.Handlers
{
    public class ViewBookInputHandler : IRequestHandler<ViewBookInputCommand, ViewBookOutputCommand>
    {

        private readonly IBookBussiness _IBookBussiness;
        public ViewBookInputHandler(IBookBussiness IBookBussiness)
        {
            _IBookBussiness = IBookBussiness;
        }

        public Task<ViewBookOutputCommand> Handle(ViewBookInputCommand request, CancellationToken cancellationToken)
        {
            ViewBookOutputCommand _ViewBookOutputCommand = new ViewBookOutputCommand();
           var BookEntity= _IBookBussiness.ViewBook(request.Id);
            _ViewBookOutputCommand.Id = BookEntity.Id;
            _ViewBookOutputCommand.PageCount = BookEntity.PageCount;
            _ViewBookOutputCommand.Consumed = BookEntity.Consumed;
            return Task.FromResult(_ViewBookOutputCommand);
        }
    }
}
