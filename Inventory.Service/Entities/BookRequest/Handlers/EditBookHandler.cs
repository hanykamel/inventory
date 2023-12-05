using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Service.Entities.BookRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.BookRequest.Handlers
{
    public class EditBookHandler : IRequestHandler<EditBookCommand, bool>
    {
        private readonly IBookBussiness _IBookBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        public EditBookHandler(IBookBussiness IBookBussiness, IStringLocalizer<SharedResource> Localizer)
        {
            _IBookBussiness = IBookBussiness;
            _Localizer = Localizer;
        }
        public Task<bool> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            // 1. check object is exist 
            // 2. check object after edit is existed 
            // 3. check if the book is used from other lists 

            if (_IBookBussiness.checkValidEdit(request.Id))
            {
                Book entity = new Book();
                entity.Id = request.Id;
                entity.BookNumber = request.BookNumber;
                entity.PageCount = request.PageCount;
                entity.Consumed = request.Consumed;
                entity.StoreId = request.StoreId;
                entity.TenantId = request.StoreId;

                List<Book> lstExistance = _IBookBussiness.CheckBookExistane(entity).ToList();


                if (lstExistance.Count > 0)
                    throw new InvalidBookException(_Localizer["InvalidBookException"]);
                return _IBookBussiness.UpdateBook(entity);
            }
            else
            {
                throw new InvalidBookException(_Localizer["invalidBookEdit"]);

            }
        }
    }
}
