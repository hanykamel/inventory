using AutoMapper;
using Inventory.Data.Models.Inquiry;
using Inventory.Service.Entities.InquiryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using System.Linq;

namespace Inventory.Service.Entities.InquiryRequest.Handlers
{
    public class EditStoreItemsBookHandler : IRequestHandler<EditStoreItemsBookCommand, bool>
    {
        private readonly IStoreItemBussiness _iStoreItemBussiness;
        public EditStoreItemsBookHandler(
            IStoreItemBussiness iStoreItemBussiness
            )
        {
            _iStoreItemBussiness = iStoreItemBussiness;
        }

        public async Task<bool> Handle(EditStoreItemsBookCommand request, CancellationToken cancellationToken)
        {
            
            return await _iStoreItemBussiness.EditStoreItemsBooksItems(request);
        
        }
    }
}

