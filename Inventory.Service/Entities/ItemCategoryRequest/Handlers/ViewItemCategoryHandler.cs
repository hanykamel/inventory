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
    public class ViewItemCategoryHandler : IRequestHandler<ViewItemCategoryCommand, ItemCategoryOutPutCommand>
    {

        private readonly IItemCategoryBussiness _IItemCategoryBussiness;
        public ViewItemCategoryHandler(IItemCategoryBussiness IItemCategoryBussiness)
        {
            _IItemCategoryBussiness = IItemCategoryBussiness;
        }
        public Task<ItemCategoryOutPutCommand> Handle(ViewItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var ItemCategoryentity = _IItemCategoryBussiness.ViewItemCategory(request.Id);

            ItemCategoryOutPutCommand _entity = new ItemCategoryOutPutCommand();
            _entity.Id = ItemCategoryentity.Id;
            _entity.Name = ItemCategoryentity.Name;
            _entity.IsActive = ItemCategoryentity.IsActive;
            return Task.FromResult(_entity);
        }
    }
}
