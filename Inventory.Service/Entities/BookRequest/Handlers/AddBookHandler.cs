using Inventory.Service.Interfaces;
using Inventory.Data.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Service.Entities.BookRequest.Commands;
using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Inventory.Service.Entities.BookRequest.Handlers
{
    public class AddBookHandler : IRequestHandler<AddBookCommand, Book>
    {
        private readonly IBookBussiness _IBookBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public AddBookHandler(IBookBussiness IBookBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IBookBussiness = IBookBussiness;
            _Localizer = Localizer;
        }
        public Task<Book> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            Book entity = new Book();
            entity.Consumed = request.Consumed;
            entity.BookNumber = request.BookNumber;
            entity.PageCount = request.PageCount;
            entity.StoreId = request.StoreId;
            entity.TenantId = request.StoreId;
            List<Book> lstExistance = _IBookBussiness.CheckBookExistane(entity).ToList();
            if (lstExistance.Count > 0)
                throw new InvalidBookException(_Localizer["InvalidBookException"]);

            return _IBookBussiness.AddNewBook(entity);
        }
    }
}
