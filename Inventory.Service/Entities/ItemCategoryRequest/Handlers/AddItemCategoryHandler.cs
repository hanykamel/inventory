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
    public class AddItemCategoryHandler : IRequestHandler<AddItemCategoryCommand, ItemCategory>
    {

        private readonly IItemCategoryBussiness _IItemCategoryBussiness;
        public AddItemCategoryHandler(IItemCategoryBussiness IItemCategoryBussiness)
        {
            _IItemCategoryBussiness = IItemCategoryBussiness;
        }
        public Task<ItemCategory> Handle(AddItemCategoryCommand request, CancellationToken cancellationToken)
        {
            ItemCategory entity = new ItemCategory();
            entity.Name = request.name;
            return _IItemCategoryBussiness.AddNewItemCategory(entity);
        }
    }
}
