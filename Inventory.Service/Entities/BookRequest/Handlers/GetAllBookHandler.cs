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
    public class GetAllBookHandler : IRequestHandler<GetAllBookCommand, List<ViewBookOutputCommand>>
    {

        private readonly IBookBussiness _IBookBussiness;
        public GetAllBookHandler(IBookBussiness IBookBussiness)
        {
            _IBookBussiness = IBookBussiness;
        }
        public Task<List<ViewBookOutputCommand>> Handle(GetAllBookCommand request, CancellationToken cancellationToken)
        {
            List<ViewBookOutputCommand> _ViewBookOutputCommand = new List<ViewBookOutputCommand>();
            var BookList = _IBookBussiness.GetAllBook();
            foreach (var item in BookList)
            {
                ViewBookOutputCommand _entity = new ViewBookOutputCommand();

                _entity.Consumed = item.Consumed;
                _entity.Id = item.Id;
                _entity.PageCount = item.PageCount;
                _ViewBookOutputCommand.Add(_entity);
            }
            return Task.FromResult(_ViewBookOutputCommand);

        }
    }
}
