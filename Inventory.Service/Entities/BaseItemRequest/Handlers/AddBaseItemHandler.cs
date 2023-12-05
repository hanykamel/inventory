using Inventory.Data.Entities;
using Inventory.Service.Entities.BaseItemRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.BaseItemRequest.Handlers
{
    public class AddBaseItemHandler : IRequestHandler<AddBaseItemCommand, long>
    {
        private readonly IBaseItemBussiness _IBaseItemBussiness;
        public AddBaseItemHandler(IBaseItemBussiness IBaseItemBussiness)
        {
            _IBaseItemBussiness = IBaseItemBussiness;
        }
        public async Task<long> Handle(AddBaseItemCommand request, CancellationToken cancellationToken)
        {
            BaseItem entity = new BaseItem();
            entity.Name = request.Name;
            entity.ShortName = request.ShortName;
            entity.ItemCategoryId = request.ItemCategoryId;
            entity.DefaultUnitId = request.DefaultUnitId;
            entity.WarningLevel = request.WarningLevel;
            entity.Consumed= request.Consumed;
            entity.Description = request.Description;
            return await _IBaseItemBussiness.AddNewBaseItem(entity);
            
        }
    }
}
