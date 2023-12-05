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
    public class GetAllItemCategoryHandler : IRequestHandler<GetAllItemCategoryCommand, List<ItemCategoryOutPutCommand>>
    {
        private readonly IItemCategoryBussiness _IItemCategoryBussiness;
        public GetAllItemCategoryHandler(IItemCategoryBussiness IItemCategoryBussiness)
        {
            _IItemCategoryBussiness = IItemCategoryBussiness;
        }
        public Task<List<ItemCategoryOutPutCommand>> Handle(GetAllItemCategoryCommand request, CancellationToken cancellationToken)
        {
            List<ItemCategoryOutPutCommand> _ItemCategoryOutPutCommand = new List<ItemCategoryOutPutCommand>();
            var ItemCategoryList = _IItemCategoryBussiness.GetAllItemCategory();
            foreach (var item in ItemCategoryList)
            {
                ItemCategoryOutPutCommand _entity = new ItemCategoryOutPutCommand();
                _entity.Id = item.Id;
                _entity.IsActive = item.IsActive;
                _entity.Name = item.Name;

                _ItemCategoryOutPutCommand.Add(_entity);
            }
            return Task.FromResult(_ItemCategoryOutPutCommand);
        }
    }
}
