using Inventory.Data.Entities;
using Inventory.Service.Entities.ItemCategoryRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ItemCategoryRequest.Handlers
{
    public class EditItemCategoryHandler : IRequestHandler<EditItemCategoryCommand, bool>
    {
        private readonly IItemCategoryBussiness _IItemCategoryBussiness;
        public EditItemCategoryHandler(IItemCategoryBussiness IItemCategoryBussiness)
        {
            _IItemCategoryBussiness = IItemCategoryBussiness;
        }
        public Task<bool> Handle(EditItemCategoryCommand request, CancellationToken cancellationToken)
        {
            return _IItemCategoryBussiness.UpdateItemCategory(new ItemCategory{ Id = request.Id, Name = request.Name });
        }
    }
}
